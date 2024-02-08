using System.Reactive.Disposables;
using System.Windows.Controls;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Controls
{
    /// <summary>
    /// Interaction logic for MapItemView.xaml
    /// </summary>
    public partial class MapListItemView : ReactiveUserControl<MapListItemViewModel>
    {
        public MapListItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(
                        ViewModel,
                        vm => vm.MapImage,
                        v => v.MapImage.Source,
                        vmToViewConverterOverride: new MapPathToSourceConverter()
                    )
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Header, v => v.HeaderText.Text).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Details, v => v.DetailsText.Text)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.Footer, v => v.FooterText.Text).DisposeWith(d);
            });
        }
    }
}
