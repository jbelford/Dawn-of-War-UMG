using DowUmg.Services.Interfaces;
using System;
using System.IO;

namespace DowUmg.Presentation.WPF.Services
{
    public class AppDataProvider : IAppDataProvider
    {
        private string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string AppDataLocation => Path.Combine(appData, "DowUmg");

        public string SettingsLocation => Path.Combine(AppDataLocation, "settings.json");

        public string DataLocation => Path.Combine(AppDataLocation, "data.db");
    }
}
