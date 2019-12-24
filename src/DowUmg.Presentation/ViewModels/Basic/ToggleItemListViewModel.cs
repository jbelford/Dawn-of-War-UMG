using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemListViewModel<T> : ReactiveObject
    {
        public ToggleItemListViewModel(string label, IEnumerable<ToggleItemViewModel<T>> items)
        {
            Label = label;
            ToggleItems = ReactiveCommand.Create(() =>
            {
                if (Items.Count > 0)
                {
                    bool isToggled = Items[0].IsToggled;
                    foreach (var item in Items)
                    {
                        item.IsToggled = !isToggled;
                    }
                }
            });

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public string Label { get; }

        public ObservableCollection<ToggleItemViewModel<T>> Items { get; } = new ObservableCollection<ToggleItemViewModel<T>>();

        public ReactiveCommand<Unit, Unit> ToggleItems { get; }
    }

    public class ToggleItemListViewModel : ToggleItemListViewModel<object>
    {
        public ToggleItemListViewModel(string label, IEnumerable<ToggleItemViewModel> items) : base(label, items)
        {
        }
    }
}
