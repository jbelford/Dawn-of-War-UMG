using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Media;

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
                this.ModName.Text = ViewModel.Module.File.UIName;
                this.ModDesc.Text = ViewModel.Module.File.Description;

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IsLoaded,
                    view => view.Text.Foreground,
                    loaded => new BrushConverter().ConvertFromString(loaded ? "ForestGreen" : "Red"))
                    .DisposeWith(d);
            });
        }
    }
}
