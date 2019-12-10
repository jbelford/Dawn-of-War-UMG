using DowUmg.Presentation.ViewModels.Generation;
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
                this.OneWayBind(ViewModel, vm => vm.GeneralTab, v => v.GeneralTab.ViewModel).DisposeWith(d);
            });
        }
    }
}
