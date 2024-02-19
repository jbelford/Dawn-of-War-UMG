using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for GameTabView.xaml
    /// </summary>
    public partial class GameTabControl : ReactiveUserControl<GameTabViewModel>
    {
        public GameTabControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.DiffOption, v => v.DiffOption.Content)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SpeedOption, v => v.SpeedOption.Content)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.RateOption, v => v.RateOption.Content)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.StartingOption, v => v.StartingOption.Content)
                    .DisposeWith(d);
            });
        }
    }
}
