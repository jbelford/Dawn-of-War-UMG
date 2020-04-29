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
            addonMaps.Sort(MapSort);

            AddonMaps = new ToggleItemListViewModel<DowMap>("Addon Maps");
            AddonMaps.SetItems(addonMaps.Select(map => new ToggleItemViewModel<DowMap>(true) { Label = $"{map.Name} [{map.Size}]", Item = map }));

            Maps = new ToggleItemListViewModel<DowMap>("Maps");
            Rules = new ToggleItemListViewModel<GameRule>("Win Conditions");

            RefreshForMod = ReactiveCommand.CreateFromTask(async (int id) =>
            {
                var (maps, rules) = await Observable.Start(() =>
                {
                    using var store = new ModsDataStore();
                    return (store.GetMaps(id).ToList(), store.GetRules(id).ToList());
                }, RxApp.TaskpoolScheduler);

                maps.Sort(MapSort);

                Maps.SetItems(maps.Select(map => new ToggleItemViewModel<DowMap>(true) { Label = $"{map.Name} [{map.Size}]", Item = map }));
                Rules.SetItems(rules.Where(rule => rule.IsWinCondition)
                        .Select(rule => new ToggleItemViewModel<GameRule>(true) { Label = rule.Name, Item = rule }));
            });

            var sizeToToggle = new Dictionary<int, ToggleItemViewModel<int>>();
            var playerToToggle = new Dictionary<int, ToggleItemViewModel<int>>();

            ToggleMapPlayerFilter = ReactiveCommand.Create((ToggleItemViewModel<int> player) =>
            {
                foreach (var map in Maps.Items.Concat(AddonMaps.Items))
                {
                    if (map.Item.Players == player.Item
                        && sizeToToggle.GetValueOrDefault(map.Item.Size) is ToggleItemViewModel<int> size
                        && size.IsToggled)
                    {
                        map.IsEnabled = player.IsToggled;
                    }
                }
            });

            ToggleMapSizeFilter = ReactiveCommand.Create((ToggleItemViewModel<int> size) =>
            {
                foreach (var map in Maps.Items.Concat(AddonMaps.Items))
                {
                    if (map.Item.Size == size.Item
                        && playerToToggle.GetValueOrDefault(map.Item.Players) is ToggleItemViewModel<int> players
                        && players.IsToggled)
                    {
                        map.IsEnabled = size.IsToggled;
                    }
                }
            });

            foreach (var players in Enumerable.Range(2, 7))
            {
                var item = new ToggleItemViewModel<int>(true) { Label = $"{players}p", Item = players };
                MapTypes.Add(item);
                playerToToggle.Add(players, item);
                item.WhenAnyValue(x => x.IsToggled)
                    .DistinctUntilChanged()
                    .Select(x => item)
                    .InvokeCommand(ToggleMapPlayerFilter);
            }

            foreach (int size in Enum.GetValues(typeof(MapSize)))
            {
                var item = new ToggleItemViewModel<int>(true) { Label = size.ToString(), Item = size };
                MapSizes.Add(item);
                sizeToToggle.Add(size, item);
                item.WhenAnyValue(x => x.IsToggled)
                    .DistinctUntilChanged()
                    .Select(x => item)
                    .InvokeCommand(ToggleMapSizeFilter);
            }

            this.WhenAnyValue(x => x.Mod)
                .Where(mod => mod != null)
                .Select(mod => mod.Id)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForMod);
        }

        [Reactive]
        public DowMod Mod { get; set; }

        public ObservableCollection<DowRace> Races { get; } = new ObservableCollection<DowRace>();

        public ToggleItemListViewModel<DowMap> Maps { get; set; }

        public ToggleItemListViewModel<DowMap> AddonMaps { get; set; }

        public ToggleItemListViewModel<GameRule> Rules { get; set; }

        public ObservableCollection<ToggleItemViewModel<int>> MapTypes { get; } = new ObservableCollection<ToggleItemViewModel<int>>();

        public ObservableCollection<ToggleItemViewModel<int>> MapSizes { get; } = new ObservableCollection<ToggleItemViewModel<int>>();

        public ReactiveCommand<int, Unit> RefreshForMod { get; }

        public ReactiveCommand<ToggleItemViewModel<int>, Unit> ToggleMapPlayerFilter { get; }

        public ReactiveCommand<ToggleItemViewModel<int>, Unit> ToggleMapSizeFilter { get; }

        private int MapSort(DowMap a, DowMap b)
        {
            return a.Players != b.Players
                ? a.Players - b.Players
                : a.Size != b.Size
                    ? a.Size - b.Size
                    : string.Compare(a.Name, b.Name);
        }
    }
}
