using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel(string label, string? tooltip = null, string? mapPath = null)
        : ReactiveObject
    {
        [Reactive]
        public bool IsToggled { get; set; } = true;

        [Reactive]
        public bool IsEnabled { get; set; } = true;

        public string Label { get; } = label;

        [Reactive]
        public bool IsShown { get; set; } = true;

        public string? ToolTip { get; } = tooltip;

        public string? MapPath { get; } = mapPath;
    }
}
