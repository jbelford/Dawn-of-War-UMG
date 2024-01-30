using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
                this.OneWayBind(ViewModel, vm => vm.MapSizes, v => v.MapSizes.ItemsSource).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MapTypes, v => v.MapTypes.ItemsSource).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Maps, v => v.Maps.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.AddonMaps, v => v.AddonMaps.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Rules, v => v.WinConditions.Content).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    vm => vm.AddonMaps.Items,
                    v => v.AddonMaps.Visibility,
                    items => items.Count > 0
                        ? System.Windows.Visibility.Visible
                        : System.Windows.Visibility.Hidden
                    ).DisposeWith(d);
            });
        }
    }
}
