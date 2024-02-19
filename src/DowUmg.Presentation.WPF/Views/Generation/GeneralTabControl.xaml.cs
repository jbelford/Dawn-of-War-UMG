using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
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
                this.WhenAnyValue(x => x.ViewModel)
                    .WhereNotNull()
                    .Do(vm =>
                    {
                        MapTypes.ItemsSource = vm.MapTypes;
                        MapSizes.ItemsSource = vm.MapSizes;
                    })
                    .Subscribe()
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
