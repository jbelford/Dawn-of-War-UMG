using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : ReactiveObject, IRoutableViewModel
    {
        public GenerationViewModel(IScreen screen)
        {
            HostScreen = screen;

            GeneralTab = new GeneralTabViewModel();
        }

        [Reactive]
        public GeneralTabViewModel GeneralTab { get; private set; }

        public string UrlPathSegment => "matchup";

        public IScreen HostScreen { get; private set; }
    }
}
