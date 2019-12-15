using DowUmg.Presentation.ViewModels.Basic;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel : DisableableOption
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
