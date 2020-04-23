using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for GenerationSettingsView.xaml
    /// </summary>
    public partial class GenerationSettingsPage : ReactiveUserControl<GenerationViewModel>
    {
        public GenerationSettingsPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Mod, v => v.ModOption.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.GeneralTab, v => v.GeneralTab.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.GameTab, v => v.GameTab.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.TeamTab, v => v.TeamTab.ViewModel).DisposeWith(d);
            });
        }
    }
}
