using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for ToggleItemListView.xaml
    /// </summary>
    public partial class ToggleItemListView : ReactiveUserControl<ToggleItemListViewModel>
    {
        public ToggleItemListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Label, v => v.HeaderLabel.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Search, v => v.SearchTextBox.Text).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Items, v => v.ToggleItemList.ItemsSource)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.ToggleItems, v => v.ToggleItemsButton)
                    .DisposeWith(d);
            });
        }
    }
}
