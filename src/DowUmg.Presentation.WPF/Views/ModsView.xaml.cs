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
                this.Bind(ViewModel, vm => vm.SelectedBaseItem, v => v.VanillaModsList.SelectedItem).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedModItem, v => v.ModsList.SelectedItem).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.ModItems, v => v.ModsList.ItemsSource).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.BaseGameItems, v => v.VanillaModsList.ItemsSource).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.RefreshMods, v => v.RefreshButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ReloadMod, v => v.loadButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ReloadMods, v => v.loadModsButton).DisposeWith(d);

                ViewModel.RefreshMods.Execute();
            });
        }
    }
}
