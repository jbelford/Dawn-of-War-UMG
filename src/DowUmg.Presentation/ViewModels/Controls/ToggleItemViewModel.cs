using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public interface IToggleItem
    {
        public bool IsToggled { get; set; }
    }

    public class ToggleItem<T> : ReactiveObject, IToggleItem
    {
        public ToggleItem(T item)
        {
            Item = item;
            IsToggled = true;
        }

        public T Item { get; set; }

        [Reactive]
        public bool IsToggled { get; set; }
    }

    public class ToggleItemViewModel : ReactiveObject
    {
        public ToggleItemViewModel(IToggleItem item, string label)
        {
            Item = item;
            Label = label;
            IsShown = true;
        }

        public IToggleItem Item { get; set; }

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
