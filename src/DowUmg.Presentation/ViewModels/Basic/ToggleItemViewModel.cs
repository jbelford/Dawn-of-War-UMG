using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel<T> : ItemViewModel<T>
    {
        public ToggleItemViewModel(bool toggled = false, bool enabled = true)
        {
            IsToggled = toggled;
            IsEnabled = enabled;
        }

        [Reactive]
        public bool IsToggled { get; set; }

        [Reactive]
        public bool IsEnabled { get; set; }
    }

    public class ToggleItemViewModel : ToggleItemViewModel<object>
    {
        public ToggleItemViewModel(bool toggled = false) : base(toggled)
        {
        }
    }
}
