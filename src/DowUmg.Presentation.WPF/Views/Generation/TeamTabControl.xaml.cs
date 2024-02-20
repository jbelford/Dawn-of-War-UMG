using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for TeamTabControl.xaml
    /// </summary>
    public partial class TeamTabControl : ReactiveUserControl<TeamTabViewModel>
    {
        public TeamTabControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Enabled, v => v.EnabledCheckBox.IsChecked)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.GlobalPlayerOptions,
                        v => v.GlobalPlayerOptions.Content
                    )
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.TeamIsEven, v => v.EvenRadio.IsChecked)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.TeamNum, v => v.TeamNum.ViewModel)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.TeamPlayerOptions, v => v.Teams.ItemsSource)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Enabled, v => v.MainContent.IsEnabled)
                    .DisposeWith(d);
            });
        }

        private void SetViewModels(TeamTabViewModel vm)
        {
            GlobalPlayerOptions.Content = vm.GlobalPlayerOptions;
        }
    }
}
