using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
                // More efficient than binding.
                this.WhenAnyValue(x => x.ViewModel)
                    .Where(x => x != null)
                    .Do(SetRoutedViewHost)
                    .Subscribe()
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GoHome, v => v.HomeButton).DisposeWith(d);
            });
        }

        private void SetRoutedViewHost(MainViewModel vm)
        {
            RoutedViewHost.Router = vm.Router;
        }
    }
}
