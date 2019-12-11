using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class CheckBoxItemModel : ReactiveObject
    {
        [Reactive]
        public string Label { get; set; }

        [Reactive]
        public bool IsChecked { get; set; }
    }
}
