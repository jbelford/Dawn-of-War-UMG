using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for CampaignPage.xaml
    /// </summary>
    public partial class CampaignView : ReactiveUserControl<CampaignViewModel>
    {
        public CampaignView()
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
