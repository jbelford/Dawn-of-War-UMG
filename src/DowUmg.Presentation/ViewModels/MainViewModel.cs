using ReactiveUI;
using System;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : RoutableReactiveObject
    {
        public MainViewModel(IScreen screen) : base(screen, "main")
        {
            SettingsAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen)));
            ModsAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new ModsViewModel(HostScreen)));
            MatchupAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new GenerationViewModel(HostScreen)));

            CloseApp = ReactiveCommand.Create(() => Environment.Exit(0));
        }

        public ReactiveCommand<Unit, Unit> CloseApp { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ModsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> MatchupAction { get; }

        public enum MenuType { Campaign, None };
    }
}
