using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : RoutableReactiveObject
    {
        private MenuType menuSelected;

        public MainViewModel(IScreen screen) : base(screen, "main")
        {
            OpenContextMenu = ReactiveCommand.Create<MenuType>(selected =>
            {
                ContextMenuIsVisible = true;
                this.menuSelected = selected;
            });

            NewAction = ReactiveCommand.Create(() => { /* todo */ });
            LoadAction = ReactiveCommand.Create(() => { /* todo */ });
            ExportAction = ReactiveCommand.Create(() => { /* todo */ });

            SettingsAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen)));
            ModsAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new ModsViewModel(HostScreen)));
            MatchupAction = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new GenerationViewModel(HostScreen)));

            CloseApp = ReactiveCommand.Create(() => Environment.Exit(0));

            this.WhenAnyValue(x => x.ContextMenuIsVisible, (isVisible) => !isVisible)
                .Subscribe(_ => this.menuSelected = MenuType.None);
        }

        public ReactiveCommand<Unit, Unit> CloseApp { get; }
        public ReactiveCommand<Unit, Unit> ExportAction { get; }
        public ReactiveCommand<Unit, Unit> LoadAction { get; }
        public ReactiveCommand<Unit, Unit> NewAction { get; }
        public ReactiveCommand<MenuType, Unit> OpenContextMenu { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ModsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> MatchupAction { get; }

        [Reactive]
        public bool ContextMenuIsVisible { get; private set; }

        public enum MenuType { Campaign, None };
    }
}
