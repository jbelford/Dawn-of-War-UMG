using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.campaignButton.Events().Click
                    .Select(args => MainViewModel.MenuType.Campaign)
                    .InvokeCommand(this, x => x.ViewModel.OpenContextMenu)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.SettingsAction, x => x.settingsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.ModsAction, x => x.modsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.MatchupAction, v => v.matchupButton)
                    .DisposeWith(d);

                this.Bind(ViewModel, viewModel => viewModel.ContextMenuIsVisible, view => view.matchupButton.ContextMenu.IsOpen)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, viewModel => viewModel.CloseApp, view => view.quitButton)
                    .DisposeWith(d);
            });
        }
    }
}
