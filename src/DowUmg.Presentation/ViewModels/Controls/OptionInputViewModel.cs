using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class OptionInputViewModel : ActivatableReactiveObject, IDisposable
    {
        private IDisposable? subscription;

        public OptionInputViewModel(
            IObservable<IChangeSet<OptionInputItemViewModel>> items,
            bool selectLast = false
        )
        {
            subscription = items.Bind(out _items).Subscribe();

            SelectedItem = selectLast ? Items.Last() : Items.First();
        }

        public OptionInputViewModel(
            IEnumerable<OptionInputItemViewModel> items,
            bool selectLast = false
        )
        {
            _items = new(new ObservableCollection<OptionInputItemViewModel>(items));
            SelectedItem = selectLast ? Items.Last() : Items.First();
        }

        [Reactive]
        public OptionInputItemViewModel SelectedItem { get; set; }

        private ReadOnlyObservableCollection<OptionInputItemViewModel>? _items;
        public ReadOnlyObservableCollection<OptionInputItemViewModel>? Items => _items;

        public void Dispose()
        {
            subscription?.Dispose();
        }
    }

    public class OptionInputItemViewModel(string label, object obj) : ReactiveObject
    {
        private readonly object _object = obj;

        [Reactive]
        public string Label { get; set; } = label;

        [Reactive]
        public bool IsEnabled { get; set; } = true;

        internal TObject GetItem<TObject>() => (TObject)_object;
    }
}
