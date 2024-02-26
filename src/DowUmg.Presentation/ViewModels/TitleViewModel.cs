using System.Reactive;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class TitleViewModel : RoutableReactiveObject
    {
        public TitleViewModel(IScreen screen)
            : base(screen, "main")
        {
            SettingsAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen))
            );
            ModsAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new ModsViewModel(HostScreen))
            );
            MatchupAction = ReactiveCommand.CreateFromObservable(
                () =>
                    HostScreen.Router.Navigate.Execute(new GenerationSettingsViewModel(HostScreen))
            );

            CampaignAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new CampaignViewModel(HostScreen))
            );

            ReactiveCommand
                .CreateFromTask(async () =>
                {
                    IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;
                    var mods = await modDataService.GetPlayableMods();
                    IsLoaded = mods.Count != 0;
                })
                .Execute();
        }

        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ModsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> MatchupAction { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> CampaignAction { get; }

        [Reactive]
        public bool IsLoaded { get; set; }
    }
}
