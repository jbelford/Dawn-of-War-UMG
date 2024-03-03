using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Data.Entities;
using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationSettingsViewModel : RoutableReactiveObject
    {
        public GenerationSettingsViewModel(IScreen screen)
            : base(screen, "generation")
        {
            GameTabViewModel = new GameTabViewModel();
            TeamTabViewModel = new TeamTabViewModel();

            IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;

            var generationState = new GenerationViewModelState();

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

            GenerateMatchupAction = ReactiveCommand.CreateFromObservable(() =>
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
                        .ToList()
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

                return HostScreen.Router.Navigate.Execute(
                    new MatchupViewModel(HostScreen, settings)
                );
            });

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ModViewModel.SelectedItem)
                    .WhereNotNull()
                    .DistinctUntilChanged()
                    .Select(mod => mod.GetItem<DowMod>())
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .InvokeCommand(RefreshMod)
                    .DisposeWith(d);
            });
        }

        public GeneralTabViewModel GeneralTabViewModel { get; }

        public GameTabViewModel GameTabViewModel { get; }

        public TeamTabViewModel TeamTabViewModel { get; }

        public OptionInputViewModel ModViewModel { get; set; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GenerateMatchupAction { get; }
    }
}
