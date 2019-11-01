using System;
using System.Reactive;
using ReactiveUI;

namespace DowUmgClient.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private bool _contextMenuIsVisible;
        private string menuSelected;

        public MainViewModel()
        {
            OpenContextMenu = ReactiveCommand.Create<string>(selected =>
            {
                ContextMenuIsVisible = true;
                this.menuSelected = selected;
            });

            NewAction = ReactiveCommand.Create(() => { /* todo */ });
            LoadAction = ReactiveCommand.Create(() => { /* todo */ });
            ExportAction = ReactiveCommand.Create(() => { /* todo */ });
            CloseApp = ReactiveCommand.Create(() => Environment.Exit(0));

            this.WhenAnyValue(x => x.ContextMenuIsVisible, (isVisible) => !isVisible)
                .Subscribe(_ => this.menuSelected = null);
        }

        public enum MenuType { Campaign, Matchup, None };

        public ReactiveCommand<Unit, Unit> CloseApp { get; }

        public bool ContextMenuIsVisible
        {
            get => _contextMenuIsVisible;
            set => this.RaiseAndSetIfChanged(ref this._contextMenuIsVisible, value);
        }

        public ReactiveCommand<Unit, Unit> ExportAction { get; }
        public ReactiveCommand<Unit, Unit> LoadAction { get; }
        public ReactiveCommand<Unit, Unit> NewAction { get; }
        public ReactiveCommand<string, Unit> OpenContextMenu { get; }
    }
}