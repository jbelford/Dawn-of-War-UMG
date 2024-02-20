using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ProportionalOptionsViewModel : ReactiveObject
    {
        public ProportionalOptionsViewModel(string name, IEnumerable<NumberInputViewModel> inputs)
        {
            Name = name;
            IsEnabled = true;
            Items = new(inputs);
        }

        [Reactive]
        public bool IsEnabled { get; set; }

        public string Name { get; }

        public ObservableCollection<NumberInputViewModel> Items { get; set; }
    }
}
