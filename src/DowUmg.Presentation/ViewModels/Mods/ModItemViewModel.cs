using DowUmg.FileFormats;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels.Mods
{
    public class ModItemViewModel : ReactiveObject
    {
        public DowModuleFile Module { get; set; }

        public LocaleStore Locales { get; set; }

        [Reactive]
        public bool IsLoaded { get; set; }
    }
}
