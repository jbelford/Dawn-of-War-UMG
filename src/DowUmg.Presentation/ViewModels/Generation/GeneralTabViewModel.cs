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
            HumanPlayers = new OptionInputViewModel<int>(Enumerable.Range(1, 8).ToArray());
            MinPlayers = new OptionInputViewModel<int>(Enumerable.Range(2, 7).ToArray());
            MaxPlayers = new OptionInputViewModel<int>(Enumerable.Range(2, 7).ToArray());
            MaxPlayers.SelectedItem = MaxPlayers.Items.Last();

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
                foreach (var minItem in MinPlayers.Items)
                {
                    minItem.IsEnabled = minItem.Content >= item.Content;
                }
                if (!MinPlayers.SelectedItem.IsEnabled)
                {
                    MinPlayers.SelectedItem = MinPlayers.Items.Where(x => x.IsEnabled).First();
                }
            });

            RefreshForMinPlayers = ReactiveCommand.Create((OptionInputItem<int> item) =>
            {
                foreach (var maxItem in MaxPlayers.Items)
                {
                    maxItem.IsEnabled = maxItem.Content >= item.Content;
                }
                if (!MaxPlayers.SelectedItem.IsEnabled)
                {
                    MaxPlayers.SelectedItem = MaxPlayers.Items.Where(x => x.IsEnabled).First();
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

            this.WhenAnyValue(x => x.HumanPlayers.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForHumanPlayers);

            this.WhenAnyValue(x => x.MinPlayers.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForMinPlayers);

            this.WhenAnyValue(x => x.MinPlayers.SelectedItem, x => x.MaxPlayers.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshMapsForRange);
        }

        [Reactive]
        public OptionInputViewModel<int> HumanPlayers { get; set; }

        [Reactive]
        public OptionInputViewModel<int> MinPlayers { get; set; }

        [Reactive]
        public OptionInputViewModel<int> MaxPlayers { get; set; }

        public ObservableCollection<ToggleItemViewModel> MapTypes { get; } = new ObservableCollection<ToggleItemViewModel>();

        public ObservableCollection<ToggleItemViewModel> MapSizes { get; } = new ObservableCollection<ToggleItemViewModel>();

        [Reactive]
        public GameOptionViewModel DiffOption { get; set; }

        [Reactive]
        public GameOptionViewModel SpeedOption { get; set; }

        [Reactive]
        public GameOptionViewModel RateOption { get; set; }

        [Reactive]
        public GameOptionViewModel StartingOption { get; set; }

        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForHumanPlayers { get; }
        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForMinPlayers { get; }
        public ReactiveCommand<(OptionInputItem<int>, OptionInputItem<int>), Unit> RefreshMapsForRange { get; }
    }
}
