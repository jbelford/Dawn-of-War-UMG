using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemListViewModel : ReactiveObject
    {
        public ToggleItemListViewModel(
            string label,
            IObservable<IChangeSet<ToggleItemViewModel>> items
        )
        {
            Label = label;
            ToggleItems = ReactiveCommand.Create(() =>
            {
                ToggleItemViewModel first = Items.FirstOrDefault();
                if (first != null)
                {
                    bool isToggled = first.IsToggled;
                    foreach (var item in Items)
                    {
                        item.IsToggled = !isToggled;
                    }
                }
            });

            FilterItems = ReactiveCommand.Create(
                (string search) =>
                {
                    foreach (var item in Items)
                    {
                        item.IsShown =
                            string.IsNullOrEmpty(search)
                            || item.Label.Contains(search, StringComparison.OrdinalIgnoreCase);
                    }
                }
            );

            items.ObserveOn(RxApp.MainThreadScheduler).Bind(out _items).Subscribe();

            this.WhenAnyValue(x => x.Search).InvokeCommand(FilterItems);
        }

        public string Label { get; }

        [Reactive]
        public string Search { get; set; }

        private ReadOnlyObservableCollection<ToggleItemViewModel> _items;
        public ReadOnlyObservableCollection<ToggleItemViewModel> Items => _items;

        public ReactiveCommand<Unit, Unit> ToggleItems { get; }

        public ReactiveCommand<string, Unit> FilterItems { get; }
    }
}
