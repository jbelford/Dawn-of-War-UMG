using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MapSelectionWindow.xaml
    /// </summary>
    public partial class MapSelectionWindow : ReactiveWindow<MapSelectionViewModel>
    {
        public MapSelectionWindow()
        {
            InitializeComponent();

            ViewModel = new MapSelectionViewModel();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.SelectedMap, v => v.MapListBox.SelectedItem)
                    .DisposeWith(d);
            });
        }
    }
}
