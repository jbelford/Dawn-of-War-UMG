using System.Reactive.Disposables;
using System.Windows;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateCampaignPage.xaml
    /// </summary>
    public partial class CreateCampaignView : ReactiveUserControl<CreateCampaignViewModel>
    {
        public CreateCampaignView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.CampaignName, v => v.CampaignNameInput.Text)
                    .DisposeWith(d);

                this.Bind(
                        ViewModel,
                        vm => vm.CampaignDescription,
                        v => v.CampaignDescriptionInput.Text
                    )
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.AddMissionCommand, v => v.AddMissionButton)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.MissionList, v => v.MissionListBox.ItemsSource)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.MissionList,
                        v => v.WarningMessage.Visibility,
                        list => list.Count > 0 ? Visibility.Hidden : Visibility.Visible
                    )
                    .DisposeWith(d);
            });
        }
    }
}
