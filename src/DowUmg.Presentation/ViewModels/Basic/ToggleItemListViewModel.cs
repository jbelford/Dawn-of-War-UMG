using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

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

            FilterItems = ReactiveCommand.Create((string search) =>
            {
                foreach (var item in Items)
                {
                    item.IsFiltered = !string.IsNullOrEmpty(search) && !item.Label.Contains(search, System.StringComparison.OrdinalIgnoreCase);
                }
            });

            this.WhenAnyValue(x => x.Search)
                .DistinctUntilChanged()
                .InvokeCommand(FilterItems);

        }

        public string Label { get; }

        [Reactive]
        public string Search { get; set; }

        public ObservableCollection<ToggleItemViewModel<T>> Items { get; } = new ObservableCollection<ToggleItemViewModel<T>>();

        public ReactiveCommand<Unit, Unit> ToggleItems { get; }

        public ReactiveCommand<string, Unit> FilterItems { get; }

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
