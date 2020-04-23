using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class MatchupViewModel : RoutableReactiveObject
    {
        public MatchupViewModel(IScreen screen) : base(screen, "matchup")
        {
        }
    }
}
