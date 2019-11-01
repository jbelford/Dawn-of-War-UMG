using System.Reactive.Disposables;
using ReactiveUI;
using DowUmgClient.ViewModels;

namespace DowUmgClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();

            this.campaignButton.CommandParameter = this.campaignButton.Content;
            this.matchupButton.CommandParameter = this.matchupButton.Content;

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, viewModel => viewModel.ContextMenuIsVisible, view => view.matchupButton.ContextMenu.IsOpen)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, viewModel => viewModel.OpenContextMenu, view => view.campaignButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, viewModel => viewModel.OpenContextMenu, view => view.matchupButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, viewModel => viewModel.CloseApp, view => view.quitButton)
                    .DisposeWith(disposables);
            });
        }
    }
}