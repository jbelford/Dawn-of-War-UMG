using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RoutingWindow : ReactiveWindow<RoutingViewModel>
    {
        public RoutingWindow()
        {
            InitializeComponent();
            ViewModel = new RoutingViewModel();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                    .DisposeWith(disposables);
            });
        }
    }
}
