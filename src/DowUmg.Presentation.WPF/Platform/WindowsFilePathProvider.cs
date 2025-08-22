using System;
using System.IO;
using System.Windows;
using DowUmg.Platform;
using Microsoft.Win32;

namespace DowUmg.Presentation.WPF.Platform
{
    public class WindowsFilePathProvider : IFilePathProvider
    {
        public WindowsFilePathProvider()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 3556750");
            SoulstormLocation = key?.GetValue("InstallLocation") as string;
            if (string.IsNullOrEmpty(SoulstormLocation))
            {
                key = Environment.Is64BitOperatingSystem
                    ? Registry.LocalMachine.OpenSubKey(
                        "SOFTWARE\\WOW6432Node\\THQ\\Dawn of War - Soulstorm"
                    )
                    : Registry.LocalMachine.OpenSubKey("SOFTWARE\\THQ\\Dawn of War - Soulstorm");
                SoulstormLocation = key?.GetValue("InstallLocation") as string;
            }

            AppDataLocation = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DowUmg"
            );

            Directory.CreateDirectory(AppDataLocation);
        }

        public string AppDataLocation { get; }

        public string ModCacheLocation => Path.Combine(AppDataLocation, "mod_cache");

        public string SettingsLocation => Path.Combine(AppDataLocation, "settings.json");

        public string DataLocation => Path.Combine(AppDataLocation, "data.db");

        public string SoulstormLocation { get; set; }

        public string CampaignsLocation => Path.Combine(AppDataLocation, "campaigns");
    }
}
