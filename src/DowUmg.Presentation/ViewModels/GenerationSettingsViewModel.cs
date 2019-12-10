using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationSettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public GenerationSettingsViewModel(IScreen screen)
        {
            HostScreen = screen;
        }

        public string UrlPathSegment => "matchup";

        public IScreen HostScreen { get; private set; }
    }
}
