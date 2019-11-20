using ReactiveUI;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class RoutingViewModel : ReactiveObject, IScreen
    {
        public RoutingViewModel()
        {
            Router = new RoutingState();

            GoToSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new SettingsViewModel(this)));

            GoBack = Router.NavigateBack;

            Router.NavigateAndReset.Execute(new MainViewModel(this));
        }

        #region Commands

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }

        #endregion Commands

        public RoutingState Router { get; }
    }
}
