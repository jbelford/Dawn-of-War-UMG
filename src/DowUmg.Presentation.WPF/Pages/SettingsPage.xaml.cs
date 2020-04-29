using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Forms;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsPage : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.SoulstormDirectory, v => v.DirectoryTextBox.Text)
                    .DisposeWith(d);

                ViewModel.GetDirectory.RegisterHandler(GetDirectoryHandler)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.SelectDirectory, v => v.SelectDirectoryButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.SaveSettings, v => v.SaveButton)
                    .DisposeWith(d);
            });
        }

        private void GetDirectoryHandler(InteractionContext<string, string?> interaction)
        {
            using var folderDialog = new FolderBrowserDialog
            {
                RootFolder = System.Environment.SpecialFolder.ProgramFilesX86
            };
            switch (folderDialog.ShowDialog())
            {
                case DialogResult.OK:
                    interaction.SetOutput(folderDialog.SelectedPath);
                    break;

                default:
                    interaction.SetOutput(null);
                    break;
            }
        }
    }
}
