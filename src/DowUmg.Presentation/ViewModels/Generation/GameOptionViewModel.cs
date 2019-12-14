using ReactiveUI;
using System.Collections.ObjectModel;

namespace DowUmg.Presentation.ViewModels
{
    public class GameOptionViewModel : ReactiveObject
    {
        public GameOptionViewModel(string name)
        {
            Header = new ToggleItemViewModel(name, true);
        }

        public ToggleItemViewModel Header { get; }

        public ObservableCollection<IReactiveObject> Items { get; } = new ObservableCollection<IReactiveObject>();
    }
}
