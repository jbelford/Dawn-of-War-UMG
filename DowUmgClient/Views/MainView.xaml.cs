using System.Reactive.Disposables;
using ReactiveUI;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Reactive.Linq;
using System.Windows.Controls;
using DowUmgClient.ViewModels;

namespace DowUmgClient.Views
{
    /// <summary>
    /// Interaction logic for TilePage.xaml
    /// </summary>
    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.campaignButton.Events().Click
                    .Select(args => MainViewModel.MenuType.Campaign)
                    .InvokeCommand(this, x => x.ViewModel.OpenContextMenu)
                    .DisposeWith(disposables);

                this.matchupButton.Events().Click
                    .Select(args => MainViewModel.MenuType.Matchup)
                    .InvokeCommand(this, x => x.ViewModel.OpenContextMenu)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, x => x.SettingsAction, x => x.settingsButton)
                    .DisposeWith(disposables);

                this.Bind(ViewModel, viewModel => viewModel.ContextMenuIsVisible, view => view.matchupButton.ContextMenu.IsOpen)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, viewModel => viewModel.CloseApp, view => view.quitButton)
                    .DisposeWith(disposables);
            });
        }
    }
}