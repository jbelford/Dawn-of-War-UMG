using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class TeamTabViewModel : ActivatableReactiveObject, IDisposable
    {
        public TeamTabViewModel()
        {
            GlobalPlayerOptions = new PlayersSelectViewModel(
                "Players",
                Enumerable.Range(1, 8),
                new RangeViewModel(2, 8)
            );

            var teamNums = new SourceList<int>();
            teamNums.AddRange(Enumerable.Range(2, 7));
            TeamNum = new OptionInputViewModel(
                teamNums
                    .Connect()
                    .Transform(num => new OptionInputItemViewModel(num.ToString(), num))
            );

            RefreshForMin = ReactiveCommand.Create(
                (OptionInputItemViewModel min) =>
                {
                    foreach (var teamItem in TeamNum.Items)
                    {
                        teamItem.IsEnabled = teamItem.GetItem<int>() <= min.GetItem<int>();
                    }
                    if (!TeamNum.SelectedItem.IsEnabled)
                    {
                        TeamNum.SelectedItem = TeamNum.Items.Last(x => x.IsEnabled);
                    }
                }
            );

            RefreshTeamList = ReactiveCommand.Create(
                (int teams) =>
                {
                    if (TeamPlayerOptions.Count < teams)
                    {
                        for (int i = TeamPlayerOptions.Count; i < teams; ++i)
                        {
                            var teamOptions = new PlayersSelectViewModel(
                                $"Team {i + 1}",
                                Enumerable.Range(0, 7).ToArray(),
                                new RangeViewModel(1, 7)
                            );

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
                }
            );

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.TeamNum.SelectedItem)
                    .WhereNotNull()
                    .DistinctUntilChanged()
                    .Select(item => item.GetItem<int>())
                    .InvokeCommand(RefreshTeamList)
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.GlobalPlayerOptions.MinMax.MinInputViewModel.SelectedItem)
                    .DistinctUntilChanged()
                    .InvokeCommand(RefreshForMin)
                    .DisposeWith(d);
            });
        }

        public void Dispose()
        {
            TeamNum.Dispose();
            GlobalPlayerOptions.Dispose();
            foreach (var team in TeamPlayerOptions)
            {
                team.Dispose();
            }
        }

        [Reactive]
        public bool Enabled { get; set; } = false;

        [Reactive]
        public bool TeamIsEven { get; set; } = true;

        public OptionInputViewModel TeamNum { get; }

        public PlayersSelectViewModel GlobalPlayerOptions { get; }
        public ObservableCollection<PlayersSelectViewModel> TeamPlayerOptions { get; } = new();
        public ReactiveCommand<OptionInputItemViewModel, Unit> RefreshForMin { get; }
        public ReactiveCommand<int, Unit> RefreshTeamList { get; }
        public ReactiveCommand<int, Unit> RefreshForMod { get; }
    }
}
