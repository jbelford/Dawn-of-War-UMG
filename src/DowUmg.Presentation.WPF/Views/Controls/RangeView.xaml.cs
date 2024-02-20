using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for RangeView.xaml
    /// </summary>
    public partial class RangeView : ReactiveUserControl<RangeViewModel>
    {
        public RangeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(
                        ViewModel,
                        vm => vm.MinInputViewModel,
                        v => v.MinOptionInput.ViewModel
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.MaxInputViewModel,
                        v => v.MaxOptionInput.ViewModel
                    )
                    .DisposeWith(d);
            });
        }
    }
}
