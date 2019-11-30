using ReactiveUI;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class ModsViewModel : ReactiveObject, IRoutableViewModel
    {
        public ModsViewModel(RoutingViewModel routing)
        {
            HostScreen = routing;
        }

        public ReactiveCommand<Unit, Unit> ReloadMod { get; }

        public ReactiveCommand<Unit, Unit> ReloadMods { get; }

        public string UrlPathSegment => "mods";

        public IScreen HostScreen { get; }
    }
}
