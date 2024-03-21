using System;
using System.Reactive;
using System.Reactive.Linq;
using DowUmg.Models;
using DowUmg.Platform;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class SettingsViewModel : RoutableReactiveObject
    {
        private readonly AppSettingsService settingsService;

        public SettingsViewModel(IScreen screen, AppSettingsService? settingsService = null)
            : base(screen, "settings")
        {
            this.settingsService =
                settingsService ?? Locator.Current.GetService<AppSettingsService>()!;

            var filePathProvider = Locator.Current.GetService<IFilePathProvider>()!;

            SavedSettings = this.settingsService.GetSettings();

            SelectDirectory = ReactiveCommand.CreateFromObservable(
                () => GetDirectory.Handle(SoulstormDirectory)
            );

            OpenAppSettingsAction = ReactiveCommand.CreateFromObservable(
                () => OpenAppSettingsFolder.Handle(filePathProvider.AppDataLocation)
            );

            SelectDirectory
                .WhereNotNull()
                .ToPropertyEx(
                    this,
                    x => x.SoulstormDirectory,
                    initialValue: SavedSettings.InstallLocation
                );

            IObservable<bool> canSave = this.WhenAnyValue(
                    x => x.SoulstormDirectory,
                    x => x.SavedSettings,
                    (dir, settings) =>
                        !string.Equals(
                            settings.InstallLocation,
                            dir,
                            StringComparison.OrdinalIgnoreCase
                        )
                )
                .DistinctUntilChanged();

            SaveSettings = ReactiveCommand.Create(
                () =>
                {
                    SavedSettings = new AppSettings() { InstallLocation = SoulstormDirectory };
                    this.settingsService.SaveSettings(SavedSettings);
                },
                canSave
            );
        }

        public ReactiveCommand<Unit, Unit> SaveSettings { get; }
        public ReactiveCommand<Unit, string?> SelectDirectory { get; }

        public ReactiveCommand<Unit, Unit> OpenAppSettingsAction { get; }

        public Interaction<string?, string?> GetDirectory { get; } = new();

        public Interaction<string, Unit> OpenAppSettingsFolder { get; } = new();

        [ObservableAsProperty]
        public string? SoulstormDirectory { get; }

        [Reactive]
        public AppSettings SavedSettings { get; set; }
    }
}
