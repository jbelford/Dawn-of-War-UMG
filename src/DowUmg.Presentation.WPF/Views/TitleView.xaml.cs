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
            });
        }
    }
}
