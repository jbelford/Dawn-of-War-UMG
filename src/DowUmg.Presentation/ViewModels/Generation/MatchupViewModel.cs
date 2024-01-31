using System;
using System.Reactive;
using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class MatchupViewModel : RoutableReactiveObject
    {
        private readonly GenerationSettings settings;
        private readonly GenerationService generationService;
        private readonly DowModLoader modLoader;

        public MatchupViewModel(
            IScreen screen,
            GenerationSettings settings,
            DowModLoader? modLoader = null,
            GenerationService? generationService = null
        )
            : base(screen, "matchup")
        {
            this.generationService =
                generationService ?? Locator.Current.GetService<GenerationService>()!;
            this.modLoader = modLoader ?? Locator.Current.GetService<DowModLoader>()!;
            this.settings = settings;

            GenerateMatchup = ReactiveCommand.Create(() =>
            {
                Matchup = this.generationService.GenerateMatchup(this.settings);
                MapImagePath = this.modLoader.GetMapImagePath(Matchup.Map);
            });

            GenerateMatchup.Execute().Subscribe();

            GoBack = HostScreen.Router.NavigateBack;
        }

        public ReactiveCommand<Unit, Unit> GenerateMatchup { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }

        [Reactive]
        public Matchup Matchup { get; set; }

        [Reactive]
        public string MapImagePath { get; set; }
    }
}
