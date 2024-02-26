﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Constants;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    internal record ToggleModel<T>(T Model, ToggleItemViewModel ToggleItem);

    public class GeneralTabViewModel : ActivatableReactiveObject, IDisposable
    {
        private DowModLoader _modLoader;

        public GeneralTabViewModel(GenerationViewModelState generationState)
        {
            _modLoader = Locator.Current.GetService<DowModLoader>()!;

            var mapTypes = Enumerable
                .Range(2, 7)
                .Select(players => new ToggleModel<int>(
                    players,
                    new ToggleItemViewModel($"{players}p")
                ))
                .ToList();

            var mapSizes = new List<ToggleModel<int>>();
            foreach (int size in Enum.GetValues(typeof(MapSize)))
            {
                mapSizes.Add(new ToggleModel<int>(size, new ToggleItemViewModel(size.ToString())));
            }

            MapTypes = new(mapTypes.Select(item => item.ToggleItem));
            MapSizes = new(mapSizes.Select(item => item.ToggleItem));
            IsAddonAllowed = generationState.IsAddonAllowed;

            MapsViewModel = new ToggleItemListViewModel(
                "Maps",
                generationState
                    .ConnectMaps()
                    .Sort(MapSort)
                    .Transform(MapToViewModelTransform)
                    .BindToObservableList(out _maps)
                    .Transform(item => item.ToggleItem)
            );

            WinConditionsViewModel = new ToggleItemListViewModel(
                "Win Conditions",
                generationState
                    .ConnectRules()
                    .Filter(rule => rule.IsWinCondition)
                    .Transform(rule => new ToggleModel<GameRule>(
                        rule,
                        new ToggleItemViewModel(rule.Name) { ToolTip = rule.Details }
                    ))
                    .BindToObservableList(out _winConditions)
                    .Transform(item => item.ToggleItem)
            );

            this.WhenActivated(d =>
            {
                foreach (var players in mapTypes)
                {
                    var item = players.Model;
                    players
                        .ToggleItem.WhenAnyValue(x => x.IsToggled)
                        .ObserveOn(RxApp.TaskpoolScheduler)
                        .Subscribe(toggled =>
                        {
                            generationState.SetPlayersAllowed(item, toggled);
                        })
                        .DisposeWith(d);
                }

                foreach (var size in mapSizes)
                {
                    var item = size.Model;
                    size.ToggleItem.WhenAnyValue(x => x.IsToggled)
                        .ObserveOn(RxApp.TaskpoolScheduler)
                        .Subscribe(toggled =>
                        {
                            generationState.SetSizeAllowed(item, toggled);
                        })
                        .DisposeWith(d);
                }

                this.WhenAnyValue(x => x.IsAddonAllowed)
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Subscribe(isAddonAlowed =>
                    {
                        generationState.IsAddonAllowed = isAddonAlowed;
                    })
                    .DisposeWith(d);
            });
        }

        public void Dispose()
        {
            MapsViewModel.Dispose();
            WinConditionsViewModel.Dispose();
        }

        private IObservableList<ToggleModel<DowMap>> _maps;
        internal IObservableList<ToggleModel<DowMap>> Maps => _maps;
        public ToggleItemListViewModel MapsViewModel { get; set; }

        private IObservableList<ToggleModel<GameRule>> _winConditions;
        internal IObservableList<ToggleModel<GameRule>> WinConditions => _winConditions;
        public ToggleItemListViewModel WinConditionsViewModel { get; set; }

        public ObservableCollection<ToggleItemViewModel> MapTypes { get; set; }

        public ObservableCollection<ToggleItemViewModel> MapSizes { get; set; }

        [Reactive]
        public bool IsAddonAllowed { get; set; }

        private ToggleModel<DowMap> MapToViewModelTransform(DowMap map) =>
            new(
                map,
                new ToggleItemViewModel($"{map.Name} [{map.Size}]")
                {
                    ToolTip = map.Details,
                    MapPath = _modLoader.GetMapImagePath(map)
                }
            );

        private static SortExpressionComparer<DowMap> MapSort =>
            SortExpressionComparer<DowMap>.Ascending(m => m.Players).ThenByAscending(m => m.Name);
    }
}
