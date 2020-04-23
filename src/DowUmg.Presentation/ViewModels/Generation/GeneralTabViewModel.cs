using DowUmg.Constants;
using DowUmg.Data;
using DowUmg.Data.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GeneralTabViewModel : ReactiveObject
    {
        public GeneralTabViewModel(List<DowMap> addonMaps)
        {
            addonMaps.Sort((a, b) => a.Players - b.Players);

            AddonMaps = new ToggleItemListViewModel<DowMap>("Addon Maps",
                addonMaps.Select(map => new ToggleItemViewModel<DowMap>(true) { Label = $"{map.Name}", Item = map }));

            foreach (var x in Enumerable.Range(2, 7))
            {
                MapTypes.Add(new ToggleItemViewModel<int>(true) { Label = $"{x}p", Item = x });
            }

            foreach (int size in Enum.GetValues(typeof(MapSize)))
            {
                MapSizes.Add(new ToggleItemViewModel<int>(true) { Label = size.ToString(), Item = size });
            }

            RefreshForMod = ReactiveCommand.CreateFromTask(async (int id) =>
            {
                var (maps, rules) = await Observable.Start(() =>
                {
                    using var store = new ModsDataStore();
                    return (store.GetMaps(id).ToList(), store.GetRules(id).ToList());
                }, RxApp.TaskpoolScheduler);

                maps.Sort((a, b) => a.Players - b.Players);

                Maps = new ToggleItemListViewModel<DowMap>("Maps", maps.Select(map => new ToggleItemViewModel<DowMap>(true) { Label = $"{map.Name}", Item = map }));
                Rules = new ToggleItemListViewModel<GameRule>("Win Conditions", rules.Where(rule => rule.IsWinCondition)
                        .Select(rule => new ToggleItemViewModel<GameRule>(true) { Label = rule.Name, Item = rule }));
            });

            this.WhenAnyValue(x => x.Mod)
                .Where(mod => mod != null)
                .Select(mod => mod.Id)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForMod);
        }

        [Reactive]
        public DowMod Mod { get; set; }

        public ObservableCollection<DowRace> Races { get; } = new ObservableCollection<DowRace>();

        [Reactive]
        public ToggleItemListViewModel<DowMap> Maps { get; set; }

        [Reactive]
        public ToggleItemListViewModel<DowMap> AddonMaps { get; set; }

        [Reactive]
        public ToggleItemListViewModel<GameRule> Rules { get; set; }

        public ObservableCollection<ToggleItemViewModel<int>> MapTypes { get; } = new ObservableCollection<ToggleItemViewModel<int>>();

        public ObservableCollection<ToggleItemViewModel<int>> MapSizes { get; } = new ObservableCollection<ToggleItemViewModel<int>>();

        public ReactiveCommand<int, Unit> RefreshForMod { get; }
    }
}
