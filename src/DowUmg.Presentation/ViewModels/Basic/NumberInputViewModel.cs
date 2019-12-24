using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class NumberInputViewModel<T> : ItemViewModel<T>
    {
        public NumberInputViewModel(int defaultValue = 0)
        {
            Input = defaultValue;
        }

        [Reactive]
        public int Input { get; set; }
    }

    public class NumberInputViewModel : NumberInputViewModel<object>
    {
        public NumberInputViewModel(int defaultValue = 0) : base(defaultValue)
        {
        }
    }
}
