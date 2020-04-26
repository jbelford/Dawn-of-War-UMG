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
                this.WhenAnyValue(x => x.ViewModel)
                    .Where(x => x != null)
                    .Do(SetViewModels)
                    .Subscribe()
                    .DisposeWith(d);
            });
        }

        private void SetViewModels(GeneralTabViewModel vm)
        {
            MapSizes.ItemsSource = vm.MapSizes;
            MapTypes.ItemsSource = vm.MapTypes;
            Maps.Content = vm.Maps;
            AddonMaps.Content = vm.AddonMaps;
            WinConditions.Content = vm.Rules;

            if (vm.AddonMaps.Items.Count > 0)
            {
                AddonMaps.Content = vm.AddonMaps;
            }
            else
            {
                AddonMaps.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
