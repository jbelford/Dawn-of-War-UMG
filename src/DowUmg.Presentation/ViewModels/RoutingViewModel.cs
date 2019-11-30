using ReactiveUI;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class RoutingViewModel : ReactiveObject, IScreen
    {
        public RoutingViewModel()
        {
            Router = new RoutingState();

            GoHome = ReactiveCommand.CreateFromObservable(() => Router.NavigateAndReset.Execute(new MainViewModel(this)));
            GoToSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new SettingsViewModel(this)));
            GotToMods = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ModsViewModel(this)));

            GoBack = Router.NavigateBack;

            GoHome.Execute();
        }

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHome { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GotToMods { get; }

        public RoutingState Router { get; }
    }
}
