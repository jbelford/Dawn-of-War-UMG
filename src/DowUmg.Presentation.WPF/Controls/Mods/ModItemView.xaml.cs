using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Controls
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

                this.OneWayBind(
                        ViewModel,
                        viewModel => viewModel.IsLoaded,
                        view => view.Text.Foreground,
                        loaded =>
                            new BrushConverter().ConvertFromString(loaded ? "ForestGreen" : "Red")
                    )
                    .DisposeWith(d);
            });
        }
    }
}
