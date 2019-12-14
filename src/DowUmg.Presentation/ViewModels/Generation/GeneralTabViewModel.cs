using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GeneralTabViewModel : ReactiveObject
    {
        private const int MinValue = 0;
        private const int MaxValue = 1000;
        private const int DefaultValue = 100;

        public GeneralTabViewModel()
        {
            HumanPlayers = 1;
            MinPlayers = 2;
            MaxPlayers = 8;

            foreach (var x in Enumerable.Range(2, 7))
            {
                MapTypes.Add(new ToggleItemViewModel($"{x}p", true));
            }

            MapSizes.Add(new ToggleItemViewModel("129", true));
            MapSizes.Add(new ToggleItemViewModel("257", true));
            MapSizes.Add(new ToggleItemViewModel("513", true));
            MapSizes.Add(new ToggleItemViewModel("1025", true));

            DiffOption = new GameOptionViewModel("Difficulty");
            DiffOption.Items.Add(new NumberInputViewModel("Easy", MinValue, MaxValue, DefaultValue));
            DiffOption.Items.Add(new NumberInputViewModel("Standard", MinValue, MaxValue, DefaultValue));
            DiffOption.Items.Add(new NumberInputViewModel("Hard", MinValue, MaxValue, DefaultValue));
            DiffOption.Items.Add(new NumberInputViewModel("Harder", MinValue, MaxValue, DefaultValue));
            DiffOption.Items.Add(new NumberInputViewModel("Insane", MinValue, MaxValue, DefaultValue));

            SpeedOption = new GameOptionViewModel("Game Speed");
            SpeedOption.Items.Add(new NumberInputViewModel("Very Slow", MinValue, MaxValue, DefaultValue));
            SpeedOption.Items.Add(new NumberInputViewModel("Slow", MinValue, MaxValue, DefaultValue));
            SpeedOption.Items.Add(new NumberInputViewModel("Normal", MinValue, MaxValue, DefaultValue));
            SpeedOption.Items.Add(new NumberInputViewModel("Fast", MinValue, MaxValue, DefaultValue));

            RateOption = new GameOptionViewModel("Resource Rate");
            RateOption.Items.Add(new NumberInputViewModel("Low", MinValue, MaxValue, DefaultValue));
            RateOption.Items.Add(new NumberInputViewModel("Standard", MinValue, MaxValue, DefaultValue));
            RateOption.Items.Add(new NumberInputViewModel("High", MinValue, MaxValue, DefaultValue));

            StartingOption = new GameOptionViewModel("Starting Resources");
            StartingOption.Items.Add(new NumberInputViewModel("Standard", MinValue, MaxValue, DefaultValue));
            StartingOption.Items.Add(new NumberInputViewModel("Quick-Start", MinValue, MaxValue, DefaultValue));
        }

        [Reactive]
        public int HumanPlayers { get; set; }

        [Reactive]
        public int MinPlayers { get; set; }

        [Reactive]
        public int MaxPlayers { get; set; }

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
    }
}
