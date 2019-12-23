using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ModItemViewModel : ReactiveObject
    {
        public UnloadedMod Module { get; set; }

        [Reactive]
        public bool IsLoaded { get; set; }
    }
}
