using ReactiveUI.Validation.Helpers;
using System.Collections.ObjectModel;

namespace DowUmg.Presentation.ViewModels
{
    public class GameOptionViewModel : ReactiveValidationObject<GameOptionViewModel>
    {
        public GameOptionViewModel(string name, params string[] options)
        {
            Header = new ToggleItemViewModel(name, true);

            foreach (var option in options)
            {
                Items.Add(new NumberInputViewModel(option, 100));
            }
        }

        public ToggleItemViewModel Header { get; }

        public ObservableCollection<NumberInputViewModel> Items { get; } = new ObservableCollection<NumberInputViewModel>();
    }
}
