using DowUmgClient.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Windows.Controls;

namespace DowUmgClient.Views
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