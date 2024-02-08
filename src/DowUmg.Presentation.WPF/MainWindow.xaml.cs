using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF
{
    /// <summary>
    /// Interaction logic for RoutingWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GoHome, v => v.HomeButton).DisposeWith(d);
            });
        }
    }
}
