using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : RoutableReactiveObject
    {
        public GenerationViewModel(IScreen screen) : base(screen, "matchup")
        {
            GeneralTab = new GeneralTabViewModel();
            GameTab = new GameTabViewModel();
            Mod = new OptionInputViewModel<string>("Dawn of War: Soulstorm");
        }

        public GeneralTabViewModel GeneralTab { get; }

        public GameTabViewModel GameTab { get; }

        public OptionInputViewModel<string> Mod { get; }
    }
}
