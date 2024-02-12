using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MapListItemViewModel : ReactiveObject
    {
        [Reactive]
        public string? MapImage { get; set; }

        [Reactive]
        public string Header { get; set; }

        [Reactive]
        public string Details { get; set; }

        [Reactive]
        public string Footer { get; set; }
    }
}
