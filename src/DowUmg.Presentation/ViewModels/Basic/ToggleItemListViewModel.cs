using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemListViewModel<T> : ReactiveObject
    {
        public ToggleItemListViewModel(string label)
        {
            Label = label;
            ToggleItems = ReactiveCommand.Create(() =>
            {
                IEnumerable<ToggleItemViewModel<T>> items = Items.Where(x => x.IsEnabled);
                ToggleItemViewModel<T> first = items.FirstOrDefault();
                if (first != null)
                {
                    bool isToggled = first.IsToggled;
                    foreach (var item in items)
                    {
                        item.IsToggled = !isToggled;
                    }
                }
            });
        }

        public string Label { get; }

        public ObservableCollection<ToggleItemViewModel<T>> Items { get; } = new ObservableCollection<ToggleItemViewModel<T>>();

        public ReactiveCommand<Unit, Unit> ToggleItems { get; }

        public void SetItems(IEnumerable<ToggleItemViewModel<T>> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }

    public class ToggleItemListViewModel : ToggleItemListViewModel<object>
    {
        public ToggleItemListViewModel(string label) : base(label)
        {
        }
    }
}
