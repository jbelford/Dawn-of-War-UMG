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

        public SettingsViewModel(RoutingViewModel routing, AppSettingsService settingsService = null)
        {
            this.settingsService = settingsService ?? Locator.Current.GetService<AppSettingsService>();

            GetDirectory = new Interaction<string, string>();
            HostScreen = routing;
            GoBack = routing.GoBack;
            SoulstormDirectory = this.settingsService.Settings.InstallLocation;

            var canSave = this.WhenAnyValue(x => x.SoulstormDirectory)
                .Select((dir) => !this.settingsService.Settings.InstallLocation.Equals(dir, StringComparison.OrdinalIgnoreCase))
                .DistinctUntilChanged();

            SaveSettings = ReactiveCommand.Create(() =>
            {
                this.settingsService.Settings = new AppSettings
                {
                    InstallLocation = SoulstormDirectory
                };
            }, canSave);

            SelectDirectory = ReactiveCommand.CreateFromTask(async () =>
            {
                string dir = await GetDirectory.Handle(SoulstormDirectory);
                if (dir != null)
                {
                    SoulstormDirectory = dir;
                }
            });

            SelectDirectory.ThrownExceptions.Subscribe(exception =>
            {
                this.Log().Warn("Error", exception);
            });
        }

        public ReactiveCommand<Unit, Unit> GoBack { get; }
        public ReactiveCommand<Unit, Unit> SaveSettings { get; }
        public ReactiveCommand<Unit, Unit> SelectDirectory { get; }

        public Interaction<string, string> GetDirectory { get; }

        [Reactive] public string SoulstormDirectory { get; private set; }

        public IScreen HostScreen { get; }
        public string UrlPathSegment => "settings";
    }
}
