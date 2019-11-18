using DowUmgClient.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using ReactiveUI;
using Splat;
using System.Reactive;
using System.Runtime.CompilerServices;

namespace DowUmgClient.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly AppSettings appSettings;

        public SettingsViewModel(RoutingViewModel routing, AppSettingsService settingsService = null)
        {
            HostScreen = routing;
            settingsService = settingsService ?? Locator.Current.GetService<AppSettingsService>();

            appSettings = settingsService.Settings;

            GoBack = routing.GoBack;

            SaveSettings = ReactiveCommand.Create(() =>
            {
                settingsService.Settings = appSettings;
            });

            SelectDirectory = ReactiveCommand.Create(() =>
            {
                var openFileDialog = new CommonOpenFileDialog()
                {
                    InitialDirectory = appSettings.InstallLocation ?? "C:\\",
                    IsFolderPicker = true
                };
                if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    SoulstormDirectory = openFileDialog.FileName;
                }
            });
        }

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public IScreen HostScreen { get; }
        public ReactiveCommand<Unit, Unit> SaveSettings { get; }

        public ReactiveCommand<Unit, Unit> SelectDirectory { get; }

        public string SoulstormDirectory
        {
            get => appSettings.InstallLocation;
            set
            {
                if (appSettings.InstallLocation != value)
                {
                    appSettings.InstallLocation = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public string UrlPathSegment => "settings";
    }
}