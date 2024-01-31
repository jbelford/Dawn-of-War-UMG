using System;
using System.Reactive;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public MainViewModel()
        {
            Router = new RoutingState();

            GoHome = ReactiveCommand.CreateFromObservable(
                () => Router.NavigateAndReset.Execute(new TitleViewModel(this))
            );

            GoBack = Router.NavigateBack;

            GoHome.Execute().Subscribe();
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHome { get; }
        public RoutingState Router { get; set; }
    }
}
