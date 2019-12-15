using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class NumberInputViewModel : ReactiveObject
    {
        public NumberInputViewModel(string label, int defaultValue = 0)
        {
            Label = label;
            Input = defaultValue;
        }

        [Reactive]
        public int Input { get; set; }

        public string Label { get; }
    }
}
