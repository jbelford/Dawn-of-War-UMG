using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for ModsView.xaml
    /// </summary>
    public partial class ModsView : ReactiveUserControl<ModsViewModel>
    {
        public ModsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ModsList.SelectedIndex, x => x.VanillaModsList.SelectedIndex,
                        (idx1, idx2) => idx1 >= 0 || idx2 >= 0)
                    .BindTo(this, view => view.loadButton.IsEnabled)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.ModItems, v => v.ModsList.ItemsSource).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.BaseGameItems, v => v.VanillaModsList.ItemsSource).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.RefreshMods, v => v.RefreshButton).DisposeWith(d);

                ViewModel.RefreshMods.Execute();
            });
        }
    }
}
