using DowUmg.Presentation.ViewModels.Basic;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace DowUmg.Presentation.ViewModels
{
    public class OptionInputViewModel<T> : ReactiveObject
    {
        public OptionInputViewModel(params T[] items)
        {
            foreach (var item in items)
            {
                Items.Add(new OptionInputItem<T>(item));
            }
            if (Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }

        [Reactive]
        public OptionInputItem<T> SelectedItem { get; set; }

        public ObservableCollection<OptionInputItem<T>> Items { get; } = new ObservableCollection<OptionInputItem<T>>();
    }

    public class OptionInputItem<T> : DisableableOption
    {
        public OptionInputItem(T content)
        {
            Content = content;
        }

        public T Content { get; set; }
    }

    public class IntOptionInputViewModel : OptionInputViewModel<int>
    {
        public IntOptionInputViewModel(params int[] items) : base(items)
        {
        }
    }

    public class IntOptionInputItem : OptionInputItem<int>
    {
        public IntOptionInputItem(int content) : base(content)
        {
        }
    }
}
