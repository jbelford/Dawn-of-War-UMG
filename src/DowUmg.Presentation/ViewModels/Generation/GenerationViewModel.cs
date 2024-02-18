using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DowUmg.Data.Entities;
using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : RoutableReactiveObject
    {
        public GenerationViewModel(IScreen screen)
            : base(screen, "generation")
        {
            GameTab = new GameTabViewModel();
            TeamTab = new TeamTabViewModel();

            IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;

            GeneralTab = new GeneralTabViewModel(modDataService.GetAddonMaps());

            Mod = new OptionInputViewModel<DowMod>(
                mod => mod.Name,
                modDataService.GetPlayableMods()
            );

            RefreshMod = ReactiveCommand.Create(
                (DowMod mod) =>
                {
                    GeneralTab.Mod = mod;
                }
            );

            GenerateMatchupAction = ReactiveCommand.CreateFromObservable(() =>
            {
                var settings = new GenerationSettings()
                {
                    Mod = Mod.SelectedItem.Item,
                    Maps = GeneralTab
                        .Maps.Items.Concat(GeneralTab.AddonMaps.Items)
                        .Where(map => map.IsEnabled && map.IsToggled)
                        .Select(map => map.Item)
                        .ToList(),
                    Rules = GeneralTab
                        .Rules.Items.Where(rule => rule.IsToggled)
                        .Select(rule => rule.Item)
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

            this.WhenAnyValue(x => x.Mod.SelectedItem)
                .DistinctUntilChanged()
                .Select(mod => mod.Item)
                .InvokeCommand(RefreshMod);
        }

        public GeneralTabViewModel GeneralTab { get; }

        public GameTabViewModel GameTab { get; }

        public TeamTabViewModel TeamTab { get; }

        public OptionInputViewModel<DowMod> Mod { get; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GenerateMatchupAction { get; }
    }
}
