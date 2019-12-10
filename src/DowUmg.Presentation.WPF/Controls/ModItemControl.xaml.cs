using DowUmg.Presentation.ViewModels.Mods;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Media;

namespace DowUmg.Presentation.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ModListControl.xaml
    /// </summary>
    public partial class ModItemControl : ReactiveUserControl<ModItemViewModel>
    {
        public ModItemControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.ModName.Text = ViewModel.Module.UIName;
                this.ModDesc.Text = ViewModel.Module.Description;

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IsLoaded,
                    view => view.Text.Foreground,
                    loaded => new BrushConverter().ConvertFromString(loaded ? "ForestGreen" : "Red"))
                    .DisposeWith(d);
            });
        }
    }
}
