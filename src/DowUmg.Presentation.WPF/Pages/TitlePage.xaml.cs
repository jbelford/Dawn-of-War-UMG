using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace DowUmg.Presentation.WPF.Pages
{
    public partial class TitlePage : ReactiveUserControl<TitleViewModel>
    {
        public TitlePage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, x => x.SettingsAction, x => x.SettingsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.ModsAction, x => x.ModsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.MatchupAction, v => v.MatchupButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, viewModel => viewModel.CloseApp, view => view.QuitButton)
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.ViewModel.IsLoaded)
                    .Do(isLoaded =>
                    {
                        WarningMessage.Visibility = isLoaded
                            ? System.Windows.Visibility.Collapsed
                            : System.Windows.Visibility.Visible;

                        MatchupButton.IsEnabled = isLoaded;
                    }).Subscribe().DisposeWith(d);
            });
        }
    }
}
