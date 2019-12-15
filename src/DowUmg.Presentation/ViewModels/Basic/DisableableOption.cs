using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels.Basic
{
    public class DisableableOption : ReactiveObject
    {
        [Reactive]
        public bool IsEnabled { get; set; } = true;
    }
}
