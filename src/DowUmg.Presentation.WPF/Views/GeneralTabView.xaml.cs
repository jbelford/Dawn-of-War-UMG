using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for GeneralTabItem.xaml
    /// </summary>
    public partial class GeneralTabItem : ReactiveUserControl<GeneralTabViewModel>
    {
        public GeneralTabItem()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.MapSizes, v => v.MapSizes.ItemsSource).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MapTypes, v => v.MapTypes.ItemsSource).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.DiffOption, v => v.DiffOption.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SpeedOption, v => v.SpeedOption.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.RateOption, v => v.RateOption.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.StartingOption, v => v.StartingOption.Content).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.HumanPlayers, v => v.HumanComboBox.Content).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.MinPlayers, v => v.MinComboBox.Content).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.MaxPlayers, v => v.MaxComboBox.Content).DisposeWith(d);
            });
        }
    }
}
