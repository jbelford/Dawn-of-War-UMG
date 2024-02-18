using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DowUmg.Constants;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class GeneralTabViewModel : ReactiveObject
    {
        private DowModLoader _modLoader;

        public GeneralTabViewModel(GenerationViewModelState generationState)
        {
            _modLoader = Locator.Current.GetService<DowModLoader>()!;

            var mapTypes = Enumerable
                .Range(2, 7)
                .Select(players => new ToggleItem<int>(players))
                .ToList();

            var mapSizes = new List<ToggleItem<int>>();
            foreach (int size in Enum.GetValues(typeof(MapSize)))
            {
                mapSizes.Add(new ToggleItem<int>(size));
            }

            MapTypes = mapTypes
                .Select(players => new ToggleItemViewModel(players, $"{players.Item}p"))
                .ToList();

            MapSizes = mapSizes
                .Select(size => new ToggleItemViewModel(size, size.Item.ToString()))
                .ToList();

            AddonMaps = new ToggleItemListViewModel(
                "Addon Maps",
                generationState.ConnectAddonMaps().Sort(MapSort).Transform(MapToViewModelTransform)
            );

            Maps = new ToggleItemListViewModel(
                "Maps",
                generationState.ConnectMainMaps().Sort(MapSort).Transform(MapToViewModelTransform)
            );

            Rules = new ToggleItemListViewModel(
                "Win Conditions",
                generationState
                    .ConnectRules()
                    .Filter(rule => rule.IsWinCondition)
                    .Transform(rule => new ToggleItemViewModel(
                        new ToggleItem<GameRule>(rule),
                        rule.Name
                    )
                    {
                        ToolTip = rule.Details
                    })
            );

            foreach (var players in mapTypes)
            {
                var item = players.Item;
                players
                    .WhenAnyValue(x => x.IsToggled)
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Subscribe(toggled =>
                    {
                        generationState.SetPlayersAllowed(item, toggled);
                    });
            }

            foreach (var size in mapSizes)
            {
                var item = size.Item;
                size.WhenAnyValue(x => x.IsToggled)
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Subscribe(toggled =>
                    {
                        generationState.SetSizeAllowed(item, toggled);
                    });
            }
        }

        public ToggleItemListViewModel Maps { get; set; }

        public ToggleItemListViewModel AddonMaps { get; set; }

        public ToggleItemListViewModel Rules { get; set; }

        public List<ToggleItemViewModel> MapTypes { get; set; }

        public List<ToggleItemViewModel> MapSizes { get; set; }

        private ToggleItemViewModel MapToViewModelTransform(DowMap map) =>
            new(new ToggleItem<DowMap>(map), $"{map.Name} [{map.Size}]")
            {
                ToolTip = map.Details,
                MapPath = _modLoader.GetMapImagePath(map)
            };

        private SortExpressionComparer<DowMap> MapSort =>
            SortExpressionComparer<DowMap>.Ascending(m => m.Players).ThenByAscending(m => m.Name);
    }
}
