using DowUmg.FileFormats;
using DowUmg.Interfaces;
using DowUmg.Models;
using Splat;
using System.IO;

namespace DowUmg.Services
{
    public class AppSettingsService
    {
        private readonly IFilePathProvider filePathProvider;

        public AppSettingsService(IFilePathProvider? provider = null)
        {
            this.filePathProvider = provider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        public AppSettings Settings
        {
            get
            {
                string settingsPath = this.filePathProvider.SettingsLocation;

                var loader = new JsonLoader<AppSettings>();

                AppSettings settings;

                if (File.Exists(settingsPath))
                {
                    settings = loader.Load(settingsPath);
                }
                else
                {
                    settings = new AppSettings
                    {
                        InstallLocation = this.filePathProvider.SoulstormLocation
                    };
                    loader.Write(settingsPath, settings);
                }

                return settings;
            }
            set
            {
                new JsonLoader<AppSettings>().Write(this.filePathProvider.SettingsLocation, value);
            }
        }
    }
}
