using DowUmg.Presentation.ViewModels;
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
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Module.UIName,
                    view => view.ModName.Text)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.Module.Description,
                    view => view.ModDesc.Text)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IsLoaded,
                    view => view.Text.Foreground,
                    loaded => new BrushConverter().ConvertFromString(loaded ? "ForestGreen" : "Red"))
                    .DisposeWith(d);
            });
        }
    }
}
