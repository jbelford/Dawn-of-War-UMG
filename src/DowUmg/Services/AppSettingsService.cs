using DowUmg.Interfaces;
using DowUmg.Models;
using Splat;
using System.IO;

namespace DowUmg.Services
{
    public class AppSettingsService
    {
        private readonly DataLoader loader;
        private readonly IFilePathProvider filePathProvider;
        private AppSettings? _settings;

        public AppSettingsService(IFilePathProvider? provider = null, DataLoader? loader = null)
        {
            this.filePathProvider = provider ?? Locator.Current.GetService<IFilePathProvider>();
            this.loader = loader ?? Locator.Current.GetService<DataLoader>();
        }

        public AppSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = InitSettings();
                }
                return new AppSettings
                {
                    InstallLocation = this._settings.InstallLocation
                };
            }
            set
            {
                _settings = value;
                loader.SaveJson(this.filePathProvider.SettingsLocation, _settings);
            }
        }

        private AppSettings InitSettings()
        {
            string settingsPath = this.filePathProvider.SettingsLocation;

            AppSettings settings;

            if (File.Exists(settingsPath))
            {
                settings = loader.LoadJson<AppSettings>(settingsPath);
            }
            else
            {
                settings = new AppSettings
                {
                    InstallLocation = this.filePathProvider.SoulstormLocation
                };
                loader.SaveJson(settingsPath, settings);
            }

            return settings;
        }
    }
}
