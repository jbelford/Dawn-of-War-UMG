using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for TitleView.xaml
    /// </summary>
    public partial class TitleView : ReactiveUserControl<TitleViewModel>
    {
        public TitleView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, x => x.SettingsAction, x => x.settingsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.ModsAction, x => x.modsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.MatchupAction, v => v.matchupButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, viewModel => viewModel.CloseApp, view => view.quitButton)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.IsLoaded, v => v.WarningMessage.Visibility,
                    loaded => loaded ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.IsLoaded, v => v.matchupButton.IsEnabled).DisposeWith(d);
            });
        }
    }
}
