using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : RoutableReactiveObject
    {
        public GenerationViewModel(IScreen screen) : base(screen, "matchup")
        {
            GeneralTab = new GeneralTabViewModel();
        }

        [Reactive]
        public GeneralTabViewModel GeneralTab { get; private set; }
    }
}
