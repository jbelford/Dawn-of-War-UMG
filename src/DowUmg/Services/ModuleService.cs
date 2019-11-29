using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DowUmg.Services
{
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
        public DowModuleFile[] GetAllModules()
        {
            string dowPath = this.filePathProvider.SoulstormLocation;

            string[] files = Directory.GetFiles(dowPath, "*.module", SearchOption.TopDirectoryOnly);

            var moduleLoader = new ModuleLoader();
            return files.Select(x => moduleLoader.Load(x)).ToArray();
        }

        public Dictionary<int, string> GetLocales(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string localePath = Path.Combine(dowPath, module.ModFolder, "Locale", "English");

            string[] files = Directory.GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);

            var ucsLoader = new UcsLoader();
            return files.Select(x => ucsLoader.Load(x))
                .Select(x => x.AsEnumerable())
                .Aggregate((a, b) => a.Concat(b))
                .GroupBy(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, x => x.Last());
        }

        public MapFile[] GetMaps(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string mapsPath = Path.Combine(dowPath, module.ModFolder, "Data", "Scenarios", "mp");

            string[] files = Directory.GetFiles(mapsPath, "*.sgb", SearchOption.AllDirectories);

            var mapsLoader = new MapLoader();
            return files.Select(x => mapsLoader.Load(x)).ToArray();
        }
    }
}
