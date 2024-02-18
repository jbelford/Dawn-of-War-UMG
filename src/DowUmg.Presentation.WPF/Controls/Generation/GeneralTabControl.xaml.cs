using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

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
                this.OneWayBind(ViewModel, vm => vm.MapSizes, v => v.MapSizes.ItemsSource)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MapTypes, v => v.MapTypes.ItemsSource)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Maps, v => v.Maps.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.AddonMaps, v => v.AddonMaps.ViewModel)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Rules, v => v.WinConditions.ViewModel)
                    .DisposeWith(d);
            });
        }
    }
}
