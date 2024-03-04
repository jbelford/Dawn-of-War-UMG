using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel : ReactiveObject
    {
        public ToggleItemViewModel(string label)
        {
            Label = label;
            IsToggled = true;
            IsShown = true;
        }

        [Reactive]
        public bool IsToggled { get; set; }

        [Reactive]
        public string Label { get; set; }

        [Reactive]
        public bool IsShown { get; set; }

        [Reactive]
        public string ToolTip { get; set; }

        [Reactive]
        public string MapPath { get; set; }
    }
}
