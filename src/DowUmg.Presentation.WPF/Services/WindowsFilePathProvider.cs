using DowUmg.Interfaces;
using Microsoft.Win32;
using System;
using System.IO;

namespace DowUmg.Presentation.WPF.Services
{
    public class WindowsFilePathProvider : IFilePathProvider
    {
        private string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string AppDataLocation => Path.Combine(appData, "DowUmg");

        public string SettingsLocation => Path.Combine(AppDataLocation, "settings.json");

        public string DataLocation => Path.Combine(AppDataLocation, "data.db");

        public string SoulstormLocation
        {
            get
            {
                RegistryKey key = Environment.Is64BitOperatingSystem
                ? Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\THQ\\Dawn of War - Soulstorm")
                : Registry.LocalMachine.OpenSubKey("SOFTWARE\\THQ\\Dawn of War - Soulstorm");

                return key.GetValue("InstallLocation") as string;
            }
        }
    }
}
