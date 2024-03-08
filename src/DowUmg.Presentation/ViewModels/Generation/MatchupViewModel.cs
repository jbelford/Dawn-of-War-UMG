using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

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

            this.WhenAnyValue(x => x.Matchup)
                .Select(match =>
                    match.Players.Select(player => new MatchupPlayerViewModel(
                        player.Position + 1,
                        player.Name,
                        player.Team,
                        player.Race
                    ))
                )
                .Select(mapped => new ObservableCollection<MatchupPlayerViewModel>(mapped))
                .ToPropertyEx(this, x => x.MatchupPlayers);
        }

        public ReactiveCommand<Unit, Unit> GenerateMatchup { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }

        [Reactive]
        public Matchup Matchup { get; set; }

        [ObservableAsProperty]
        public ObservableCollection<MatchupPlayerViewModel> MatchupPlayers { get; set; }

        [Reactive]
        public string MapImagePath { get; set; }
    }

    public class MatchupPlayerViewModel(int position, string name, int team, string? race = null)
        : ReactiveObject
    {
        public int Position { get; } = position;

        public string Name { get; } = name;

        public int Team { get; } = team;

        public string? Race { get; } = race;
    }
}
