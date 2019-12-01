using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace DowUmg.Services
{
    /// <summary>
    /// A service for finding and loading files
    /// </summary>
    public class ModuleService
    {
        private readonly IFilePathProvider filePathProvider;

        public ModuleService(IFilePathProvider? filePathProvider = null)
        {
            this.filePathProvider = filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        /// <summary>
        /// Detects and returns Dawn of War Module files.
        /// </summary>
        /// <exception cref="IOException" />
        public IEnumerable<DowModuleFile> GetAllModules()
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            var moduleLoader = new ModuleLoader();
            return GetFiles(dowPath, "*.module", SearchOption.TopDirectoryOnly).Select(file => moduleLoader.Load(file));
        }

        public Locales? GetLocales(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string localePath = Path.Combine(dowPath, module.ModFolder, "Locale", "English");

            string[] files = GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                return null;
            }

            var ucsLoader = new UcsLoader();
            return files.Select(x => ucsLoader.Load(x))
                .Aggregate((a, b) => a.Concat(b));
        }

        public IEnumerable<MapFile> GetMaps(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string mapsPath = Path.Combine(dowPath, module.ModFolder, "Data", "Scenarios", "mp");

            var mapsLoader = new MapLoader();
            return GetFiles(mapsPath, "*.sgb", SearchOption.TopDirectoryOnly).Select(file => mapsLoader.Load(file));
        }

        private string[] GetFiles(string path, string searchPattern, SearchOption option)
        {
            try
            {
                return Directory.GetFiles(path, searchPattern, option);
            }
            catch (DirectoryNotFoundException)
            {
                return new string[0];
            }
        }
    }
}
