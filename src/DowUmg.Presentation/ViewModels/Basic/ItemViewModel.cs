using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ItemViewModel<T> : ReactiveObject
    {
        [Reactive]
        public T Item { get; set; }

        [Reactive]
        public string Label { get; set; }
    }

    public class ItemViewModel : ItemViewModel<object> { }
}
