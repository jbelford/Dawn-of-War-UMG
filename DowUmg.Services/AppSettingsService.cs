using DowUmg.Services.Interfaces;
using DowUmg.Services.Models;
using Splat;
using System.IO;

namespace DowUmg.Services
{
    public class AppSettingsService
    {
        private AppSettings _settings;
        private DataLoader loader;
        private IAppDataProvider appDataProvider;
        private IDowPathService pathService;

        public AppSettingsService(IAppDataProvider provider = null, IDowPathService pathService = null, DataLoader loader = null)
        {
            this.appDataProvider = provider ?? Locator.Current.GetService<IAppDataProvider>();
            this.pathService = pathService ?? Locator.Current.GetService<IDowPathService>();
            this.loader = loader ?? Locator.Current.GetService<DataLoader>();
        }

        public AppSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = initSettings();
                }
                return new AppSettings
                {
                    InstallLocation = this._settings.InstallLocation
                };
            }
            set
            {
                _settings = value;
                loader.SaveJson(this.appDataProvider.SettingsLocation, _settings);
            }
        }

        private AppSettings initSettings()
        {
            string settingsPath = this.appDataProvider.SettingsLocation;

            AppSettings settings;

            if (File.Exists(settingsPath))
            {
                settings = loader.LoadJson<AppSettings>(settingsPath);
            }
            else
            {
                settings = new AppSettings
                {
                    InstallLocation = pathService.GetSSPath()
                };
                loader.SaveJson(settingsPath, settings);
            }

            return settings;
        }
    }
}
