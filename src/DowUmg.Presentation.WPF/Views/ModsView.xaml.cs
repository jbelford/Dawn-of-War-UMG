using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
                this.WhenAnyValue(x => x.ModsList.SelectedIndex)
                    .Select(x => x >= 0)
                    .BindTo(this, view => view.loadButton.IsEnabled)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel,
                    vm => vm.ModItems,
                    v => v.ModsList.ItemsSource).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.BaseGameItems, v => v.VanillaModsList.ItemsSource).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.RefreshMods, v => v.RefreshButton).DisposeWith(d);

                ViewModel.RefreshMods.Execute();
            });
        }
    }
}
