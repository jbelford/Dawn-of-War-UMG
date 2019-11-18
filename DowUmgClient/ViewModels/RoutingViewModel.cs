using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DowUmgClient.ViewModels
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

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
        public RoutingState Router { get; }
    }
}