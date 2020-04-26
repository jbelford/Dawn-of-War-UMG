using DowUmg.Models;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class SettingsViewModel : RoutableReactiveObject
    {
        private readonly AppSettingsService settingsService;

        public SettingsViewModel(IScreen screen, AppSettingsService? settingsService = null) : base(screen, "settings")
        {
            this.settingsService = settingsService ?? Locator.Current.GetService<AppSettingsService>();

            SavedSettings = this.settingsService.GetSettings();

            SelectDirectory = ReactiveCommand.CreateFromObservable(() => GetDirectory.Handle(SoulstormDirectory));

            SelectDirectory.Where(dir => dir != null)
                .ToPropertyEx(this, x => x.SoulstormDirectory, initialValue: SavedSettings.InstallLocation);

            IObservable<bool> canSave = this.WhenAnyValue(x => x.SoulstormDirectory, x => x.SavedSettings,
                    (dir, settings) => !string.Equals(settings.InstallLocation, dir, StringComparison.OrdinalIgnoreCase))
                .DistinctUntilChanged();

            SaveSettings = ReactiveCommand.Create(() =>
            {
                SavedSettings = new AppSettings()
                {
                    InstallLocation = SoulstormDirectory
                };
                this.settingsService.SaveSettings(SavedSettings);
            }, canSave);
        }

        public ReactiveCommand<Unit, Unit> SaveSettings { get; }
        public ReactiveCommand<Unit, string?> SelectDirectory { get; }

        public Interaction<string, string?> GetDirectory { get; } = new Interaction<string, string?>();

        public extern string SoulstormDirectory { [ObservableAsProperty] get; }

        [Reactive]
        public AppSettings SavedSettings { get; set; }
    }
}
