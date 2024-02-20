using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for OptionInputView.xaml
    /// </summary>
    public partial class OptionInputView : ReactiveUserControl<OptionInputViewModel>
    {
        public OptionInputView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Items, v => v.ComboBox.ItemsSource)
                    .DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedItem, v => v.ComboBox.SelectedItem)
                    .DisposeWith(d);
            });
        }
    }
}
