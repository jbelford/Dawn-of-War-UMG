using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel(string label) : ReactiveObject
    {
        [Reactive]
        public bool IsToggled { get; set; } = true;

        [Reactive]
        public string Label { get; set; } = label;

        [Reactive]
        public bool IsShown { get; set; } = true;

        [Reactive]
        public string ToolTip { get; set; }

        [Reactive]
        public string MapPath { get; set; }
    }
}
