using Splat;
using System;
using System.IO;

namespace DowUmgClient.Models
{
    public class AppSettings
    {
        public string InstallLocation { get; set; }
    }

    public class AppSettingsService
    {
        private static readonly string DIRECTORY = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/DowUmg";
        private static readonly string SETTINGS_PATH = $"{DIRECTORY}/settings.json";

        private AppSettings _settings;
        private DataLoader loader;
        private DowPathService pathService;

        public AppSettingsService(DowPathService pathService = null, DataLoader loader = null)
        {
            this.pathService = pathService ?? Locator.Current.GetService<DowPathService>();
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
                return _settings;
            }
            set
            {
                _settings = value;
                loader.Save(SETTINGS_PATH, _settings);
            }
        }

        private AppSettings initSettings()
        {
            Directory.CreateDirectory(DIRECTORY);

            AppSettings settings;

            if (File.Exists(SETTINGS_PATH))
            {
                settings = loader.Load<AppSettings>(SETTINGS_PATH);
            }
            else
            {
                settings = new AppSettings
                {
                    InstallLocation = pathService.GetSSPath()
                };
                loader.Save(SETTINGS_PATH, settings);
            }

            return settings;
        }
    }
}