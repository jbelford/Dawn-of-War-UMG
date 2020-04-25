using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class MatchupViewModel : RoutableReactiveObject
    {
        private readonly GenerationSettings settings;
        private readonly GenerationService generationService;

        public MatchupViewModel(IScreen screen, GenerationSettings settings, GenerationService? generationService = null)
            : base(screen, "matchup")
        {
            this.generationService = generationService ?? Locator.Current.GetService<GenerationService>();
            this.settings = settings;

            Matchup = this.generationService.GenerateMatchup(this.settings);

            GenerateMatchup = ReactiveCommand.Create(() =>
            {
                Matchup = this.generationService.GenerateMatchup(this.settings);
            });

            GoBack = HostScreen.Router.NavigateBack;
        }

        public ReactiveCommand<Unit, Unit> GenerateMatchup { get; }

        public ReactiveCommand<Unit, Unit> GoBack { get; }

        [Reactive]
        public Matchup Matchup { get; set; }
    }
}
