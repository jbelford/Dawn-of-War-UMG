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
    public class GenerationSettingsViewModel : RoutableReactiveObject, IActivatableViewModel
    {
        public GenerationSettingsViewModel(IScreen screen)
            : base(screen, "generation")
        {
            GameTab = new GameTabViewModel();
            TeamTab = new TeamTabViewModel();

            IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;

            var generationState = new GenerationViewModelState(modDataService.GetAddonMaps());

            GeneralTab = new GeneralTabViewModel(generationState);

            Mod = new OptionInputViewModel<DowMod>(
                mod => mod.Name,
                modDataService.GetPlayableMods()
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
                    Mod = Mod.SelectedItem.Item,
                    Maps = GeneralTab
                        .Maps.Items.Concat(GeneralTab.AddonMaps.Items)
                        .Where(map => map.ToggleItem.IsToggled)
                        .Select(map => map.Model)
                        .ToList(),
                    Rules = GeneralTab
                        .WinConditions.Items.Where(rule => rule.ToggleItem.IsToggled)
                        .Select(rule => rule.Model)
                        .ToList()
                };

                foreach (var diff in GameTab.DiffOption.Items)
                {
                    settings.GameDifficultyTickets[(int)diff.Item] = diff.Input;
                }

                foreach (var speed in GameTab.SpeedOption.Items)
                {
                    settings.GameSpeedTickets[(int)speed.Item] = speed.Input;
                }

                foreach (var rate in GameTab.RateOption.Items)
                {
                    settings.ResourceRateTickets[(int)rate.Item] = rate.Input;
                }

                foreach (var start in GameTab.StartingOption.Items)
                {
                    settings.StartResourceTickets[(int)start.Item] = start.Input;
                }

                return HostScreen.Router.Navigate.Execute(
                    new MatchupViewModel(HostScreen, settings)
                );
            });

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.Mod.SelectedItem)
                    .DistinctUntilChanged()
                    .Select(mod => mod.Item)
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .InvokeCommand(RefreshMod)
                    .DisposeWith(d);
            });
        }

        public GeneralTabViewModel GeneralTab { get; }

        public GameTabViewModel GameTab { get; }

        public TeamTabViewModel TeamTab { get; }

        public OptionInputViewModel<DowMod> Mod { get; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GenerateMatchupAction { get; }
    }
}
