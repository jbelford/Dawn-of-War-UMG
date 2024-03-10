using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Forms;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using Wpf.Ui.Controls;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(
                        ViewModel,
                        vm => vm.SoulstormDirectory,
                        v => v.DirectoryTextBox.Text
                    )
                    .DisposeWith(d);

                ViewModel.GetDirectory.RegisterHandler(GetDirectoryHandler).DisposeWith(d);
                ViewModel
                    .OpenAppSettingsFolder.RegisterHandler(context =>
                    {
                        Process.Start("explorer.exe", context.Input);
                        context.SetOutput(Unit.Default);
                    })
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.SelectDirectory, v => v.SelectDirectoryButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.SaveSettings, v => v.SaveButton)
                    .DisposeWith(d);

                this.BindCommand(
                        ViewModel,
                        vm => vm.OpenAppSettingsAction,
                        v => v.AppSettingsButton
                    )
                    .DisposeWith(d);

                this.WhenAnyObservable(x => x.ViewModel.SaveSettings.CanExecute)
                    .Subscribe(canExecute =>
                    {
                        SaveButton.Appearance = canExecute
                            ? ControlAppearance.Primary
                            : ControlAppearance.Dark;

                        SaveIcon.Filled = canExecute;
                    })
                    .DisposeWith(d);
            });
        }

        private void GetDirectoryHandler(IInteractionContext<string, string> interaction)
        {
            using var folderDialog = new FolderBrowserDialog
            {
                RootFolder = System.Environment.SpecialFolder.ProgramFilesX86,
                InitialDirectory = interaction.Input
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
