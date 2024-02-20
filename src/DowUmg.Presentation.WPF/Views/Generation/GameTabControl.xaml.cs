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
                this.OneWayBind(ViewModel, vm => vm.DiffOptionViewModel, v => v.DiffOption.Content)
                    .DisposeWith(d);
                this.OneWayBind(
                        ViewModel,
                        vm => vm.SpeedOptionViewModel,
                        v => v.SpeedOption.Content
                    )
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.RateOptionViewModel, v => v.RateOption.Content)
                    .DisposeWith(d);
                this.OneWayBind(
                        ViewModel,
                        vm => vm.StartingOptionViewModel,
                        v => v.StartingOption.Content
                    )
                    .DisposeWith(d);
            });
        }
    }
}
