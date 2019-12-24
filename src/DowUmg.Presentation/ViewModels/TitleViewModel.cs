using DowUmg.Data;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class TitleViewModel : RoutableReactiveObject
    {
        public TitleViewModel(IScreen screen) : base(screen, "main")
        {
            using var store = new ModsDataStore();
            IsLoaded = store.GetPlayableMods().Any();

            SettingsAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen)));
            ModsAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new ModsViewModel(HostScreen)));
            MatchupAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new GenerationViewModel(HostScreen)));

            CloseApp = ReactiveCommand.Create(() => Environment.Exit(0));
        }

        public ReactiveCommand<Unit, Unit> CloseApp { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ModsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> MatchupAction { get; }

        [Reactive]
        public bool IsLoaded { get; set; }

        public enum MenuType { Campaign, None };
    }
}
