using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        private MenuType menuSelected;

        public MainViewModel(RoutingViewModel routing)
        {
            HostScreen = routing;

            OpenContextMenu = ReactiveCommand.Create<MenuType>(selected =>
            {
                ContextMenuIsVisible = true;
                this.menuSelected = selected;
            });

            NewAction = ReactiveCommand.Create(() => { /* todo */ });
            LoadAction = ReactiveCommand.Create(() => { /* todo */ });
            ExportAction = ReactiveCommand.Create(() => { /* todo */ });

            SettingsAction = routing.GoToSettings;
            ModsAction = routing.GotToMods;

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

        [Reactive]
        public bool ContextMenuIsVisible { get; private set; }

        public IScreen HostScreen { get; }

        public string UrlPathSegment => "main";

        public enum MenuType { Campaign, Matchup, None };
    }
}
