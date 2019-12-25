using DowUmg.Presentation.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using ReactiveUI;
using System.Reactive.Disposables;

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
                this.OneWayBind(ViewModel, vm => vm.SoulstormDirectory, view => view.directoryTextBox.Text)
                    .DisposeWith(d);

                this.ViewModel.GetDirectory
                    .RegisterHandler(interaction =>
                    {
                        using var openFileDialog = new CommonOpenFileDialog()
                        {
                            InitialDirectory = interaction.Input ?? "C:\\",
                            IsFolderPicker = true
                        };

                        switch (openFileDialog.ShowDialog())
                        {
                            case CommonFileDialogResult.Ok:
                                interaction.SetOutput(openFileDialog.FileName);
                                break;

                            default:
                                interaction.SetOutput(null);
                                break;
                        }
                    })
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.SelectDirectory, x => x.selectDirectoryButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, x => x.SaveSettings, x => x.saveButton)
                    .DisposeWith(d);
            });
        }
    }
}
