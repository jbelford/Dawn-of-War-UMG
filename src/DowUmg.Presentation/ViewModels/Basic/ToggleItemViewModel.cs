using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel : ReactiveObject
    {
        public ToggleItemViewModel(string label, bool toggled = false)
        {
            Label = label;
            IsToggled = toggled;
        }

        public string Label { get; }

        [Reactive]
        public bool IsToggled { get; set; }
    }
}
