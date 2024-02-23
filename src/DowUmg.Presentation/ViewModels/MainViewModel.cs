using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public MainViewModel()
        {
            Router = new RoutingState();

            GoHome = ReactiveCommand.CreateFromObservable(
                () => Router.NavigateAndReset.Execute(new TitleViewModel(this)),
                Router.CurrentViewModel.Select(vm => vm is not TitleViewModel)
            );

            GoBack = Router.NavigateBack;

            GoHome.Execute().Subscribe();

            GoHome.CanExecute.ToPropertyEx(this, x => x.IsHomeEnabled);
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoHome { get; }
        public RoutingState Router { get; set; }

        [ObservableAsProperty]
        public bool IsHomeEnabled { get; }
    }
}
