using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Data.Entities;
using DowUmg.Models;
using DowUmg.Services;
using DynamicData;
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

            var generationState = new GenerationViewModelState(modDataService.GetAddonMaps());

            GeneralTabViewModel = new GeneralTabViewModel(generationState);

            var playableMods = new SourceList<DowMod>();
            playableMods.AddRange(modDataService.GetPlayableMods());
            ModViewModel = new OptionInputViewModel(
                playableMods.Connect().Transform(mod => new OptionInputItemViewModel(mod.Name, mod))
            );

            RefreshMod = ReactiveCommand.Create(
                (DowMod mod) =>
                {
                    generationState.RefreshForMod(mod.Id);
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

                GeneralTabViewModel.DisposeWith(d);
                TeamTabViewModel.DisposeWith(d);
                ModViewModel.DisposeWith(d);
            });
        }

        public GeneralTabViewModel GeneralTabViewModel { get; }

        public GameTabViewModel GameTabViewModel { get; }

        public TeamTabViewModel TeamTabViewModel { get; }

        public OptionInputViewModel ModViewModel { get; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GenerateMatchupAction { get; }
    }
}
