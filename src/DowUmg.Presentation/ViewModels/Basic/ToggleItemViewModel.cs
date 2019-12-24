using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel<T> : DisableableReactiveObject
    {
        public ToggleItemViewModel(string label, T item, bool toggled = false)
        {
            Label = label;
            IsToggled = toggled;
        }

        public string Label { get; }

        public T Item { get; }

        [Reactive]
        public bool IsToggled { get; set; }
    }

    public class ToggleItemViewModel : ToggleItemViewModel<object>
    {
        public ToggleItemViewModel(string label, object item, bool toggled = false) : base(label, item, toggled)
        {
        }
    }
}
