using DowUmg.Constants;
using DowUmg.Data;
using DowUmg.Data.Entities;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GeneralTabViewModel : ReactiveObject
    {
        public GeneralTabViewModel()
        {
            GlobalPlayerOptions = new PlayersSelectViewModel("Players",
                new OptionInputViewModel<int>(Enumerable.Range(1, 8).ToArray()),
                new RangeViewModel(2, 8));

            TeamNum = new OptionInputViewModel<int>(Enumerable.Range(2, 7).ToArray());

            foreach (var x in Enumerable.Range(2, 7))
            {
                MapTypes.Add(new ToggleItemViewModel<int>(true) { Label = $"{x}p", Item = x });
            }

            foreach (int size in Enum.GetValues(typeof(MapSize)))
            {
                MapSizes.Add(new ToggleItemViewModel<int>(true) { Label = size.ToString(), Item = size });
            }

            RefreshMapsForRange = ReactiveCommand.Create(((OptionInputItem<int> min, OptionInputItem<int> max) minMax) =>
            {
                for (int i = 0; i < MapTypes.Count; ++i)
                {
                    ToggleItemViewModel<int> mapType = MapTypes[i];
                    int mapPlayers = i + 2;
                    bool wasDisabled = !mapType.IsEnabled;
                    mapType.IsEnabled = mapPlayers >= minMax.min.Item && mapPlayers <= minMax.max.Item;
                    if (!mapType.IsEnabled)
                    {
                        mapType.IsToggled = false;
                    }
                    else if (wasDisabled)
                    {
                        mapType.IsToggled = true;
                    }
                }
            });

            RefreshForMin = ReactiveCommand.Create((OptionInputItem<int> min) =>
            {
                foreach (var teamItem in TeamNum.Items)
                {
                    teamItem.IsEnabled = teamItem.Item <= min.Item;
                }
                if (!TeamNum.SelectedItem.IsEnabled)
                {
                    TeamNum.SelectedItem = TeamNum.Items.Last(x => x.IsEnabled);
                }
            });

            RefreshTeamList = ReactiveCommand.Create((int teams) =>
            {
                if (TeamPlayerOptions.Count < teams)
                {
                    for (int i = TeamPlayerOptions.Count; i < teams; ++i)
                    {
                        var teamOptions = new PlayersSelectViewModel($"Team {i + 1}",
                            new OptionInputViewModel<int>(Enumerable.Range(0, 7).ToArray()),
                            new RangeViewModel(1, 7));

                        TeamPlayerOptions.Add(teamOptions);
                    }
                }
                else
                {
                    for (int i = TeamPlayerOptions.Count - 1; i >= teams; --i)
                    {
                        TeamPlayerOptions.RemoveAt(i);
                    }
                }
            });

            RefreshForMod = ReactiveCommand.CreateFromTask(async (int id) =>
            {
                Races.Clear();
                var (races, maps, rules) = await Observable.Start(() =>
                {
                    using var store = new ModsDataStore();
                    return (store.GetRaces(id).ToList(), store.GetMaps(id).ToList(), store.GetRules(id).ToList());
                }, RxApp.TaskpoolScheduler);

                maps.Sort((a, b) => a.Players - b.Players);

                Races.AddRange(races);

                Maps = new ToggleItemListViewModel<DowMap>("Maps", maps.Select(map => new ToggleItemViewModel<DowMap>(true) { Label = $"{map.Name}", Item = map }));
                Rules = new ToggleItemListViewModel<GameRule>("Win Conditions", rules.Where(rule => rule.IsWinCondition)
                        .Select(rule => new ToggleItemViewModel<GameRule>(true) { Label = rule.Name, Item = rule }));
            });

            this.WhenAnyValue(x => x.GlobalPlayerOptions.MinMax.MinInput.SelectedItem,
                    x => x.GlobalPlayerOptions.MinMax.MaxInput.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshMapsForRange);

            this.WhenAnyValue(x => x.TeamNum.SelectedItem)
                .DistinctUntilChanged()
                .Select(item => item.Item)
                .InvokeCommand(RefreshTeamList);

            this.WhenAnyValue(x => x.GlobalPlayerOptions.MinMax.MinInput.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForMin);

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
        public ToggleItemListViewModel<GameRule> Rules { get; set; }

        [Reactive]
        public bool TeamIsEven { get; set; } = true;

        public OptionInputViewModel<int> TeamNum { get; }

        public PlayersSelectViewModel GlobalPlayerOptions { get; }
        public ObservableCollection<PlayersSelectViewModel> TeamPlayerOptions { get; } = new ObservableCollection<PlayersSelectViewModel>();

        public ObservableCollection<ToggleItemViewModel<int>> MapTypes { get; } = new ObservableCollection<ToggleItemViewModel<int>>();

        public ObservableCollection<ToggleItemViewModel<int>> MapSizes { get; } = new ObservableCollection<ToggleItemViewModel<int>>();

        public ReactiveCommand<(OptionInputItem<int>, OptionInputItem<int>), Unit> RefreshMapsForRange { get; }
        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForMin { get; }
        public ReactiveCommand<int, Unit> RefreshTeamList { get; }
        public ReactiveCommand<int, Unit> RefreshForMod { get; }
    }
}
