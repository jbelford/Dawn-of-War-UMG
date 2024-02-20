using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for ToggleItemControl.xaml
    /// </summary>
    public partial class ToggleItemControl : ReactiveUserControl<ToggleItemViewModel>
    {
        public ToggleItemControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .WhereNotNull()
                    .Do(vm =>
                    {
                        CheckBox.Content = vm.Label;

                        if (vm.ToolTip != null)
                        {
                            ToolTipText.Text = vm.ToolTip;
                            ToolTip.Visibility = Visibility.Visible;
                            if (vm.MapPath != null)
                            {
                                ToolTipImage.Visibility = Visibility.Visible;
                                ToolTipImage.Source = new MapPathToSourceConverter().CreateSource(
                                    vm.MapPath
                                );
                            }
                        }
                    })
                    .Subscribe()
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.IsShown,
                        v => v.CheckBox.Visibility,
                        vmToViewConverterOverride: new BoolToVisibilityConverter()
                    )
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.IsToggled, v => v.CheckBox.IsChecked).DisposeWith(d);
            });
        }
    }
}
