using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
            Mod = new OptionInputViewModel<string>("Dawn of War: Soulstorm");

            GlobalPlayerOptions = new PlayersSelectViewModel("Players",
                new OptionInputViewModel<int>(Enumerable.Range(1, 8).ToArray()),
                new RangeViewModel(2, 8));

            TeamNum = new OptionInputViewModel<int>(Enumerable.Range(2, 7).ToArray());

            foreach (var x in Enumerable.Range(2, 7))
            {
                MapTypes.Add(new ToggleItemViewModel($"{x}p", true));
            }

            MapSizes.Add(new ToggleItemViewModel("129", true));
            MapSizes.Add(new ToggleItemViewModel("257", true));
            MapSizes.Add(new ToggleItemViewModel("513", true));
            MapSizes.Add(new ToggleItemViewModel("1025", true));

            DiffOption = new GameOptionViewModel("Difficulty", "Easy", "Standard", "Hard", "Harder", "Insane");
            SpeedOption = new GameOptionViewModel("Game Speed", "Very Slow", "Slow", "Normal", "Fast");
            RateOption = new GameOptionViewModel("Resource Rate", "Low", "Standard", "High");
            StartingOption = new GameOptionViewModel("Starting Resources", "Standard", "Quick-Start");

            RefreshForHumanPlayers = ReactiveCommand.Create((OptionInputItem<int> item) =>
            {
                OptionInputViewModel<int> minInput = GlobalPlayerOptions.MinMax.MinInput;
                foreach (var minItem in minInput.Items)
                {
                    minItem.IsEnabled = minItem.Content >= item.Content;
                }
                if (!minInput.SelectedItem.IsEnabled)
                {
                    minInput.SelectedItem = minInput.Items.Where(x => x.IsEnabled).First();
                }
            });

            RefreshMapsForRange = ReactiveCommand.Create(((OptionInputItem<int> min, OptionInputItem<int> max) minMax) =>
            {
                for (int i = 0; i < MapTypes.Count; ++i)
                {
                    ToggleItemViewModel mapType = MapTypes[i];
                    int mapPlayers = i + 2;
                    bool wasDisabled = !mapType.IsEnabled;
                    mapType.IsEnabled = mapPlayers >= minMax.min.Content && mapPlayers <= minMax.max.Content;
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

            RefreshTeamList = ReactiveCommand.Create((int teams) =>
            {
                if (TeamPlayerOptions.Count < teams)
                {
                    for (int i = TeamPlayerOptions.Count; i < teams; ++i)
                    {
                        var teamOptions = new PlayersSelectViewModel($"Team {i + 1}",
                            new OptionInputViewModel<int>(Enumerable.Range(0, 7).ToArray()),
                            new RangeViewModel(0, 7));

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

            this.WhenAnyValue(x => x.GlobalPlayerOptions.Humans.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForHumanPlayers);

            this.WhenAnyValue(x => x.GlobalPlayerOptions.MinMax.MinInput.SelectedItem,
                    x => x.GlobalPlayerOptions.MinMax.MaxInput.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshMapsForRange);

            this.WhenAnyValue(x => x.TeamNum.SelectedItem)
                .DistinctUntilChanged()
                .Select(item => item.Content)
                .InvokeCommand(RefreshTeamList);
        }

        public OptionInputViewModel<string> Mod { get; }

        [Reactive]
        public bool TeamIsEven { get; set; } = true;

        public OptionInputViewModel<int> TeamNum { get; }

        public PlayersSelectViewModel GlobalPlayerOptions { get; }
        public ObservableCollection<PlayersSelectViewModel> TeamPlayerOptions { get; } = new ObservableCollection<PlayersSelectViewModel>();

        public ObservableCollection<ToggleItemViewModel> MapTypes { get; } = new ObservableCollection<ToggleItemViewModel>();

        public ObservableCollection<ToggleItemViewModel> MapSizes { get; } = new ObservableCollection<ToggleItemViewModel>();

        public GameOptionViewModel DiffOption { get; set; }
        public GameOptionViewModel SpeedOption { get; set; }
        public GameOptionViewModel RateOption { get; set; }
        public GameOptionViewModel StartingOption { get; set; }

        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForHumanPlayers { get; }
        public ReactiveCommand<(OptionInputItem<int>, OptionInputItem<int>), Unit> RefreshMapsForRange { get; }

        public ReactiveCommand<int, Unit> RefreshTeamList { get; }
    }
}
