using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;
using System;
using System.Linq;
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
                this.OneWayBind(ViewModel, vm => vm.ModItems, v => v.ModsList.ItemsSource)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.UnplayableMods,
                        v => v.ModsListPlaceholder.Visibility,
                        vmToViewConverterOverride: new BoolToVisibilityConverter())
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.BaseGameItems,
                        v => v.VanillaModsList.ItemsSource
                    )
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.RefreshMods, v => v.RefreshButton)
                    .DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.ReloadMods, v => v.loadModsButton)
                    .DisposeWith(d);

                this.WhenAnyObservable(x => x.ViewModel.ReloadMods.IsExecuting)
                    .Subscribe(isLoading =>
                    {
                        ModLoadBar.Visibility = isLoading
                            ? System.Windows.Visibility.Visible
                            : System.Windows.Visibility.Collapsed;
                    })
                    .DisposeWith(d);
            });
        }
    }
}
