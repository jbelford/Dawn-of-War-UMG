using ReactiveUI;
using System;
using System.Reactive;

namespace DowUmg.Presentation.ViewModels
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        private bool _contextMenuIsVisible;
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

            CloseApp = ReactiveCommand.Create(() => Environment.Exit(0));

            this.WhenAnyValue(x => x.ContextMenuIsVisible, (isVisible) => !isVisible)
                .Subscribe(_ => this.menuSelected = MenuType.None);
        }

        #region Commands

        public ReactiveCommand<Unit, Unit> CloseApp { get; }
        public ReactiveCommand<Unit, Unit> ExportAction { get; }
        public ReactiveCommand<Unit, Unit> LoadAction { get; }
        public ReactiveCommand<Unit, Unit> NewAction { get; }
        public ReactiveCommand<MenuType, Unit> OpenContextMenu { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }

        #endregion Commands

        public bool ContextMenuIsVisible
        {
            get => _contextMenuIsVisible;
            set => this.RaiseAndSetIfChanged(ref this._contextMenuIsVisible, value);
        }

        public IScreen HostScreen { get; }

        public string UrlPathSegment => "main";

        public enum MenuType { Campaign, Matchup, None };
    }
}
