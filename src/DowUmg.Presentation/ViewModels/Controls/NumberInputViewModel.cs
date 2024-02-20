using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class NumberInputViewModel(string label, int defaultValue = 0) : ReactiveObject
    {
        [Reactive]
        public int Input { get; set; } = defaultValue;

        [Reactive]
        public string Label { get; set; } = label;
    }
}
