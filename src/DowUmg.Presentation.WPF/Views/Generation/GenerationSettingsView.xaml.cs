using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for GenerationSettingsView.xaml
    /// </summary>
    public partial class GenerationSettingsView : ReactiveUserControl<GenerationViewModel>
    {
        public GenerationSettingsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Mod, v => v.ModOption.Content).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.GeneralTab, v => v.GeneralTab.ViewModel).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.GameTab, v => v.GameTab.ViewModel).DisposeWith(d);
            });
        }
    }
}
