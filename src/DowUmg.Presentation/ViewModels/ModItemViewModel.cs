using DowUmg.FileFormats;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class ModItemViewModel : ReactiveObject
    {
        public DowModuleFile Module { get; set; }

        public bool IsLoaded { get; set; }
    }
}
