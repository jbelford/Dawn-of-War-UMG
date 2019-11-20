using DowUmg.Services.Interfaces;
using Microsoft.Win32;
using System;

namespace DowUmg.Presentation.WPF.Models
{
    public class DowPathService : IDowPathService
    {
        // Possible locations:
        // HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\thq\dawn of war - soulstorm (64-bit machine)
        // HKEY_LOCAL_MACHINE\SOFTWARE\thq\dawn of war - soulstorm (32-bit machine)
        public string GetSSPath()
        {
            RegistryKey key = Environment.Is64BitOperatingSystem
                ? Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\THQ\\Dawn of War - Soulstorm")
                : Registry.LocalMachine.OpenSubKey("SOFTWARE\\THQ\\Dawn of War - Soulstorm");

            return key.GetValue("InstallLocation") as string;
        }
    }
}