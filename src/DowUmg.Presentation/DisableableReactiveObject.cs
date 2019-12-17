using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation
{
    public class DisableableReactiveObject : ReactiveObject
    {
        [Reactive]
        public bool IsEnabled { get; set; } = true;
    }
}
