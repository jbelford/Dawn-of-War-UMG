using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for CampaignPage.xaml
    /// </summary>
    public partial class CampaignPage : ReactiveUserControl<CampaignViewModel>
    {
        public CampaignPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.NewCampaignAction, v => v.NewCampaignButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.LoadCampaignAction, v => v.LoadCampaignButton)
                    .DisposeWith(d);

                this.BindCommand(
                        ViewModel,
                        vm => vm.CreateCampaignAction,
                        v => v.CreateCampaignButton
                    )
                    .DisposeWith(d);
            });
        }
    }
}
