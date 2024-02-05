using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class OptionInputViewModel<T> : ReactiveObject
    {
        public OptionInputViewModel(Func<T, string> toString, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Items.Add(new OptionInputItem<T>() { Label = toString.Invoke(item), Item = item });
            }
            if (Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }

        public OptionInputViewModel(IEnumerable<T> items)
            : this((T item) => item.ToString(), items) { }

        [Reactive]
        public OptionInputItem<T> SelectedItem { get; set; }

        public ObservableCollection<OptionInputItem<T>> Items { get; } =
            new ObservableCollection<OptionInputItem<T>>();
    }

    public class OptionInputItem<T> : ItemViewModel<T>
    {
        public OptionInputItem(bool enabled = true)
        {
            IsEnabled = enabled;
        }

        [Reactive]
        public bool IsEnabled { get; set; }
    }

    public class OptionInputViewModel : OptionInputViewModel<object>
    {
        public OptionInputViewModel(params object[] items)
            : base(items) { }
    }

    public class OptionInputItem : OptionInputItem<object> { }
}
