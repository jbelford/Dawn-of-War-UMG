﻿using System.IO;
using DowUmg.FileFormats;
using DowUmg.Models;
using DowUmg.Platform;
using Splat;

namespace DowUmg.Services
{
    public class AppSettingsService
    {
        private readonly IFilePathProvider filePathProvider;

        public AppSettingsService(IFilePathProvider? provider = null)
        {
            this.filePathProvider = provider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        public AppSettings GetSettings()
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

        public void SaveSettings(AppSettings settings)
        {
            new JsonLoader<AppSettings>().Write(this.filePathProvider.SettingsLocation, settings);
        }
    }
}
