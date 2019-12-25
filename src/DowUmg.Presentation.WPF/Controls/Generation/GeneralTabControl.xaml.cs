using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Controls
{
    /// <summary>
    /// Interaction logic for GeneralTabView.xaml
    /// </summary>
    public partial class GeneralTabControl : ReactiveUserControl<GeneralTabViewModel>
    {
        public GeneralTabControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.GlobalPlayerOptions, v => v.GlobalPlayerOptions.Content).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.TeamIsEven, v => v.EvenRadio.IsChecked).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.TeamNum, v => v.TeamNum.Content).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.TeamPlayerOptions, v => v.Teams.ItemsSource).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.MapSizes, v => v.MapSizes.ItemsSource).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MapTypes, v => v.MapTypes.ItemsSource).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Maps, v => v.Maps.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Rules, v => v.WinConditions.Content).DisposeWith(d);
            });
        }
    }
}
