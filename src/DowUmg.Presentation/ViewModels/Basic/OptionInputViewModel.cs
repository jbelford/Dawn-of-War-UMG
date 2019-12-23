using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;

namespace DowUmg.Presentation.ViewModels
{
    public class OptionInputViewModel<T> : ReactiveObject
    {
        public OptionInputViewModel(Func<T, string> toString, params T[] items)
        {
            foreach (var item in items)
            {
                Items.Add(new OptionInputItem<T>(item, toString.Invoke(item)));
            }
            if (Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }

        public OptionInputViewModel(params T[] items) : this((T item) => item!.ToString(), items)
        {
        }

        [Reactive]
        public OptionInputItem<T> SelectedItem { get; set; }

        public ObservableCollection<OptionInputItem<T>> Items { get; } = new ObservableCollection<OptionInputItem<T>>();
    }

    public class OptionInputItem<T> : DisableableReactiveObject
    {
        public OptionInputItem(T content, string display)
        {
            Content = content;
            Display = display;
        }

        public T Content { get; set; }

        public string Display { get; set; }
    }

    public class OptionInputViewModel : OptionInputViewModel<object>
    {
        public OptionInputViewModel(params object[] items) : base(items)
        {
        }
    }

    public class OptionInputItem : OptionInputItem<object>
    {
        public OptionInputItem(object content, string display) : base(content, display)
        {
        }
    }
}
