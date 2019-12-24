using ReactiveUI;
using System;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public MainViewModel()
        {
            Router = new RoutingState();

            GoHome = ReactiveCommand.CreateFromObservable(() => Router.NavigateAndReset.Execute(new TitleViewModel(this)));

            GoBack = Router.NavigateBack;

            GoHome.Execute().Subscribe();
        }

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHome { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoToSettings { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GotToMods { get; }

        public RoutingState Router { get; }
    }
}
