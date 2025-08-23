using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    public partial class TitleView : ReactiveUserControl<TitleViewModel>
    {
        public TitleView()
        {
            InitializeComponent();

            #if RELEASE
            CampaignButton.Visibility = System.Windows.Visibility.Collapsed;
            #endif

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, x => x.SettingsAction, x => x.SettingsButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.ModsAction, x => x.ModsButton).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.MatchupAction, v => v.MatchupButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.CampaignAction, v => v.CampaignButton)
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.ViewModel.IsLoaded)
                    .Do(isLoaded =>
                    {
                        WarningMessage.Visibility = isLoaded
                            ? System.Windows.Visibility.Collapsed
                            : System.Windows.Visibility.Visible;
                    })
                    .Subscribe()
                    .DisposeWith(d);
            });
        }
    }
}
