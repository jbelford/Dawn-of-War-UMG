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
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel, IEnableLogger
    {
        private readonly AppSettingsService settingsService;

        public SettingsViewModel(RoutingViewModel routing, AppSettingsService? settingsService = null)
        {
            this.settingsService = settingsService ?? Locator.Current.GetService<AppSettingsService>();

            GetDirectory = new Interaction<string, string?>();
            HostScreen = routing;
            GoBack = routing.GoBack;
            SavedSettings = this.settingsService.Settings;
            SoulstormDirectory = SavedSettings.InstallLocation!;

            IObservable<bool> canSave = this.WhenAnyValue(x => x.SoulstormDirectory, x => x.SavedSettings,
                    (dir, settings) => !string.Equals(settings.InstallLocation, dir, StringComparison.OrdinalIgnoreCase))
                .DistinctUntilChanged();

            SaveSettings = ReactiveCommand.Create(() =>
            {
                var newSettings = new AppSettings()
                {
                    InstallLocation = SoulstormDirectory
                };
                this.settingsService.Settings = newSettings;

                return newSettings;
            }, canSave);

            SaveSettings.Subscribe(settings => SavedSettings = settings);

            SelectDirectory = ReactiveCommand.CreateFromObservable(() => GetDirectory.Handle(SoulstormDirectory));

            SelectDirectory.Where(x => x != null).Subscribe(dir => SoulstormDirectory = dir!);

            SelectDirectory.ThrownExceptions.Subscribe(exception =>
            {
                this.Log().Warn("Error", exception);
            });
        }

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, AppSettings> SaveSettings { get; }
        public ReactiveCommand<Unit, string?> SelectDirectory { get; }

        public Interaction<string, string?> GetDirectory { get; }

        [Reactive]
        public string SoulstormDirectory { get; private set; }

        [Reactive]
        public AppSettings SavedSettings { get; private set; }

        public IScreen HostScreen { get; }
        public string UrlPathSegment => "settings";
    }
}
