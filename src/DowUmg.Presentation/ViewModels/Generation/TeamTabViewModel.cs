using DowUmg.Data.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class TeamTabViewModel : ReactiveObject
    {
        public TeamTabViewModel()
        {
            GlobalPlayerOptions = new PlayersSelectViewModel("Players",
                Enumerable.Range(1, 8), new RangeViewModel(2, 8));

            TeamNum = new OptionInputViewModel<int>(Enumerable.Range(2, 7).ToArray());

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
                            Enumerable.Range(0, 7).ToArray(), new RangeViewModel(1, 7));

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

            //RefreshForMod = ReactiveCommand.CreateFromTask(async (int id) =>
            //{
            //    Races.Clear();
            //    var (races, maps, rules) = await Observable.Start(() =>
            //    {
            //        using var store = new ModsDataStore();
            //        return (store.GetRaces(id).ToList(), store.GetMaps(id).ToList(), store.GetRules(id).ToList());
            //    }, RxApp.TaskpoolScheduler);

            //    maps.Sort((a, b) => a.Players - b.Players);

            //    Races.AddRange(races);

            //    await GlobalPlayerOptions.RefreshForRaces.Execute(races);

            //    foreach (var team in TeamPlayerOptions)
            //    {
            //        await team.RefreshForRaces.Execute(races);
            //    }

            //    Maps = new ToggleItemListViewModel<DowMap>("Maps", maps.Select(map => new ToggleItemViewModel<DowMap>(true) { Label = $"{map.Name}", Item = map }));
            //    Rules = new ToggleItemListViewModel<GameRule>("Win Conditions", rules.Where(rule => rule.IsWinCondition)
            //            .Select(rule => new ToggleItemViewModel<GameRule>(true) { Label = rule.Name, Item = rule }));
            //});

            //RefreshMapsForRange = ReactiveCommand.Create(((OptionInputItem<int> min, OptionInputItem<int> max) minMax) =>
            //{
            //    for (int i = 0; i < MapTypes.Count; ++i)
            //    {
            //        ToggleItemViewModel<int> mapType = MapTypes[i];
            //        int mapPlayers = i + 2;
            //        bool wasDisabled = !mapType.IsEnabled;
            //        mapType.IsEnabled = mapPlayers >= minMax.min.Item && mapPlayers <= minMax.max.Item;
            //        if (!mapType.IsEnabled)
            //        {
            //            mapType.IsToggled = false;
            //        }
            //        else if (wasDisabled)
            //        {
            //            mapType.IsToggled = true;
            //        }
            //    }
            //});

            //this.WhenAnyValue(x => x.GlobalPlayerOptions.MinMax.MinInput.SelectedItem,
            //        x => x.GlobalPlayerOptions.MinMax.MaxInput.SelectedItem)
            //    .DistinctUntilChanged()
            //    .InvokeCommand(RefreshMapsForRange);

            this.WhenAnyValue(x => x.TeamNum.SelectedItem)
                .DistinctUntilChanged()
                .Select(item => item.Item)
                .InvokeCommand(RefreshTeamList);

            this.WhenAnyValue(x => x.GlobalPlayerOptions.MinMax.MinInput.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForMin);

            //this.WhenAnyValue(x => x.Mod)
            //    .Where(mod => mod != null)
            //    .Select(mod => mod.Id)
            //    .DistinctUntilChanged()
            //    .InvokeCommand(RefreshForMod);
        }

        [Reactive]
        public bool Enabled { get; set; } = false;

        [Reactive]
        public bool TeamIsEven { get; set; } = true;

        public ObservableCollection<DowRace> Races { get; } = new ObservableCollection<DowRace>();

        public OptionInputViewModel<int> TeamNum { get; }

        public PlayersSelectViewModel GlobalPlayerOptions { get; }
        public ObservableCollection<PlayersSelectViewModel> TeamPlayerOptions { get; } = new ObservableCollection<PlayersSelectViewModel>();
        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForMin { get; }
        public ReactiveCommand<int, Unit> RefreshTeamList { get; }
        public ReactiveCommand<int, Unit> RefreshForMod { get; }
        //public ReactiveCommand<(OptionInputItem<int>, OptionInputItem<int>), Unit> RefreshMapsForRange { get; }
    }
}
