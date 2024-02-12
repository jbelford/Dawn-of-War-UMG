using System;
using System.IO;
using DowUmg.Interfaces;
using Microsoft.Win32;

namespace DowUmg.Presentation.WPF.Services
{
    public class WindowsFilePathProvider : IFilePathProvider
    {
        private string AppDataLocation { get; }

        public WindowsFilePathProvider()
        {
            RegistryKey key = Environment.Is64BitOperatingSystem
                ? Registry.LocalMachine.OpenSubKey(
                    "SOFTWARE\\WOW6432Node\\THQ\\Dawn of War - Soulstorm"
                )
                : Registry.LocalMachine.OpenSubKey("SOFTWARE\\THQ\\Dawn of War - Soulstorm");

            SoulstormLocation = key.GetValue("InstallLocation") as string;

            AppDataLocation = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DowUmg"
            );

            Directory.CreateDirectory(AppDataLocation);
        }

        public string ModCacheLocation => Path.Combine(AppDataLocation, "mod_cache");

        public string SettingsLocation => Path.Combine(AppDataLocation, "settings.json");

        public string DataLocation => Path.Combine(AppDataLocation, "data.db");

        public string SoulstormLocation { get; }

        public string CampaignsLocation => Path.Combine(AppDataLocation, "campaigns");
    }
}
