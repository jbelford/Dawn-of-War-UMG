using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Data.Entities;
using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationSettingsViewModel : RoutableReactiveObject
    {
        private readonly GenerationViewModelState generationState = new();

        public GenerationSettingsViewModel(IScreen screen)
            : base(screen, "generation")
        {
            GameTabViewModel = new GameTabViewModel();

            IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;

            TeamTabViewModel = new TeamTabViewModel(generationState);
            GeneralTabViewModel = new GeneralTabViewModel(generationState);

            Observable.StartAsync(
                async () =>
                {
                    var mods = await modDataService.GetPlayableMods();
                    ModViewModel = new OptionInputViewModel(
                        mods.Select(mod => new OptionInputItemViewModel(mod.Name, mod))
                    );
                },
                RxApp.TaskpoolScheduler
            );

            RefreshMod = ReactiveCommand.CreateFromTask(
                (DowMod mod) =>
                {
                    return generationState.RefreshForMod(mod.Id);
                }
            );

            this.WhenActivated(d =>
            {
                var canGenerateMatchup = this.WhenAnyValue(
                        x => x.GeneralTabViewModel.MapsViewModel.ToggledCount,
                        x => x.TeamTabViewModel.RacesViewModel.ToggledCount
                    )
                    .Select(result => result.Item1 > 0 && result.Item2 > 0)
                    .ObserveOn(RxApp.MainThreadScheduler);

                GenerateMatchupAction = ReactiveCommand.CreateFromObservable(
                    () =>
                        HostScreen.Router.Navigate.Execute(
                            new MatchupViewModel(HostScreen, CreateGenerationSettings())
                        ),
                    canGenerateMatchup
                );

                this.WhenAnyValue(x => x.ModViewModel.SelectedItem)
                    .WhereNotNull()
                    .DistinctUntilChanged()
                    .Select(mod => mod.GetItem<DowMod>())
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .InvokeCommand(RefreshMod)
                    .DisposeWith(d);

                this.WhenAnyValue(
                        x => x.TeamTabViewModel.PlayerCountInput.SelectedItem,
                        x => x.TeamTabViewModel.MinComputers.SelectedItem
                    )
                    .DistinctUntilChanged()
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Subscribe(result =>
                    {
                        var (selectedPlayers, selectedMinComputer) = result;
                        OnTeamUpdated(
                            selectedPlayers.GetItem<int>(),
                            selectedMinComputer.GetItem<int>()
                        );
                    })
                    .DisposeWith(d);
            });
        }

        private GenerationSettings CreateGenerationSettings()
        {
            var settings = new GenerationSettings()
            {
                Mod = ModViewModel.SelectedItem.GetItem<DowMod>(),
                Maps = GeneralTabViewModel
                    .Maps.Items.Where(map => map.ToggleItem.IsToggled)
                    .Select(map => map.Model)
                    .ToList(),
                Rules = GeneralTabViewModel
                    .WinConditions.Items.Where(rule => rule.ToggleItem.IsToggled)
                    .Select(rule => rule.Model)
                    .ToList(),
                Races = TeamTabViewModel
                    .Races.Items.Where(race => race.ToggleItem.IsToggled)
                    .Select(race => race.Model)
                    .ToList(),
                Players = TeamTabViewModel.Players.Select(player => player.Name.Trim()).ToList(),
                MinComputer = TeamTabViewModel.MinComputers.SelectedItem.GetItem<int>(),
                MaxComputer = TeamTabViewModel.MaxComputers.SelectedItem.GetItem<int>(),
                MinTeams = TeamTabViewModel.MinTeams.SelectedItem.GetItem<int>(),
                MaxTeams = TeamTabViewModel.MaxTeams.SelectedItem.GetItem<int>(),
                RandomPositions = TeamTabViewModel.RandomPositions,
                OneRaceTeams = TeamTabViewModel.OneRaceTeams,
            };

            foreach (var diff in GameTabViewModel.DiffOption)
            {
                settings.GameDifficultyTickets[(int)diff.Model] = diff.NumberInput.Input;
            }

            foreach (var speed in GameTabViewModel.SpeedOption)
            {
                settings.GameSpeedTickets[(int)speed.Model] = speed.NumberInput.Input;
            }

            foreach (var rate in GameTabViewModel.RateOption)
            {
                settings.ResourceRateTickets[(int)rate.Model] = rate.NumberInput.Input;
            }

            foreach (var start in GameTabViewModel.StartingOption)
            {
                settings.StartResourceTickets[(int)start.Model] = start.NumberInput.Input;
            }

            return settings;
        }

        private void OnTeamUpdated(int selectedPlayers, int selectedMinComputer)
        {
            int minType = selectedPlayers + selectedMinComputer;
            bool updated = false;
            for (int i = 0; i < GeneralTabViewModel.MapTypes.Count; ++i)
            {
                var isEnabled = minType <= i + 2;
                GeneralTabViewModel.MapTypes[i].IsEnabled = isEnabled;
                updated |= generationState.SetPlayersAllowed(i + 2, isEnabled);
            }
            if (updated)
            {
                generationState.RefreshFilters();
            }
        }

        [Reactive]
        public int SelectedTab { get; set; } = 0;

        public GeneralTabViewModel GeneralTabViewModel { get; }

        public GameTabViewModel GameTabViewModel { get; }

        public TeamTabViewModel TeamTabViewModel { get; }

        public OptionInputViewModel ModViewModel { get; set; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GenerateMatchupAction { get; set; }
    }
}
