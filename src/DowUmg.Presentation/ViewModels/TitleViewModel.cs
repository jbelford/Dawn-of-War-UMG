using System.Reactive;
using System.Reactive.Linq;
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
            var isLoadedObservable = Observable.StartAsync(
                async () =>
                {
                    IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;
                    var mods = await modDataService.GetPlayableMods();
                    return mods.Count != 0;
                },
                RxApp.TaskpoolScheduler
            );

            SettingsAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen))
            );
            ModsAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new ModsViewModel(HostScreen))
            );
            MatchupAction = ReactiveCommand.CreateFromObservable(
                () =>
                    HostScreen.Router.Navigate.Execute(new GenerationSettingsViewModel(HostScreen)),
                isLoadedObservable
            );

            CampaignAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new CampaignViewModel(HostScreen)),
                isLoadedObservable
            );

            isLoadedObservable.ToPropertyEx(this, x => x.IsLoaded);
        }

        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ModsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> MatchupAction { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> CampaignAction { get; }

        [ObservableAsProperty]
        public bool IsLoaded { get; }
    }
}
