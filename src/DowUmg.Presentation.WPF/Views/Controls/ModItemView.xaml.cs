using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using Wpf.Ui.Controls;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for ModListControl.xaml
    /// </summary>
    public partial class ModItemView : ReactiveUserControl<ModItemViewModel>
    {
        public ModItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .Where(x => x != null)
                    .Do(vm =>
                    {
                        ModName.Text = vm.Module.File.UIName;
                        ModDesc.Text = vm.Module.File.Description;
                    })
                    .Subscribe()
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.ViewModel.IsLoaded)
                    .Subscribe(loaded =>
                    {
                        var color =
                            new BrushConverter().ConvertFromString(
                                loaded ? "ForestGreen" : "OrangeRed"
                            ) as Brush;
                        ModIcon.Symbol = loaded
                            ? SymbolRegular.BoxCheckmark24
                            : SymbolRegular.BoxDismiss24;
                        ModIcon.Foreground = color;
                        ModName.Foreground = color;
                        ModDesc.Foreground = color;
                    })
                    .DisposeWith(d);
            });
        }
    }
}
