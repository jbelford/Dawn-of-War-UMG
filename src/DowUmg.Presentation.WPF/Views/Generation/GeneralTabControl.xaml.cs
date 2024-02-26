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
                        Maps.ViewModel = vm.MapsViewModel;
                        WinConditions.ViewModel = vm.WinConditionsViewModel;
                    })
                    .Subscribe()
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.IsAddonAllowed, v => v.AddonMapsToggle.IsChecked)
                    .DisposeWith(d);
            });
        }
    }
}
