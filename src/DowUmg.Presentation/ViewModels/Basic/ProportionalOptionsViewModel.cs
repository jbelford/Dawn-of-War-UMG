using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace DowUmg.Presentation.ViewModels
{
    public class ProportionalOptionsViewModel<T> : ReactiveObject
    {
        public ProportionalOptionsViewModel(string name, Func<T, string> toString, params T[] options)
        {
            Name = name;
            IsEnabled = true;

            foreach (var option in options)
            {
                Items.Add(new NumberInputViewModel<T>(100) { Label = toString.Invoke(option), Item = option });
            }
        }

        public ProportionalOptionsViewModel(string name, params T[] options)
            : this(name, (T item) => item!.ToString(), options)
        {
        }

        [Reactive]
        public bool IsEnabled { get; set; }

        public string Name { get; }

        public ObservableCollection<NumberInputViewModel<T>> Items { get; } = new ObservableCollection<NumberInputViewModel<T>>();
    }

    public class ProportionalOptionsViewModel : ProportionalOptionsViewModel<object>
    {
        public ProportionalOptionsViewModel(string name, Func<object, string> toString, params object[] options)
            : base(name, toString, options)
        {
        }
    }
}
