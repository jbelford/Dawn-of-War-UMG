using DowUmgClient.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace DowUmgClient.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.SoulstormDirectory, view => view.directoryTextBox.Text)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SelectDirectory, view => view.selectDirectoryButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, x => x.GoBack, x => x.cancelButton)
                    .DisposeWith(disposables);
            });
        }
    }
}