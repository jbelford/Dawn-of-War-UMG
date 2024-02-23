using DowUmg.Presentation.ViewModels;
using Wpf.Ui.Controls;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            MainView.ViewModel = new MainViewModel();
        }
    }
}
