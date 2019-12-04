using DowUmg.Data.Entities;
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
    public class ModuleService : IEnableLogger
    {
        private readonly IFilePathProvider filePathProvider;
        private readonly IFullLogger logger;

        public ModuleService(IFilePathProvider? filePathProvider = null)
        {
            this.filePathProvider = filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
            this.logger = this.Log();
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

        public LocaleStore GetLocales(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string localePath = Path.Combine(dowPath, module.ModFolder, "Locale", "English");

            string[] files = GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);

            var ucsLoader = new LocaleLoader();
            return new LocaleStore(files.Select(x => ucsLoader.Load(x)).ToArray());
        }

        public IEnumerable<DowMap> GetMaps(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string mapsPath = Path.Combine(dowPath, module.ModFolder, "Data", "Scenarios", "mp");

            ISet<string> images = GetFiles(mapsPath, "*.tga", SearchOption.TopDirectoryOnly)
                .Select(x => Path.GetFileName(x))
                .ToHashSet();

            var mapsLoader = new MapLoader();

            foreach (string file in GetFiles(mapsPath, "*.sgb", SearchOption.TopDirectoryOnly))
            {
                string? image = GetImage(images, file);
                if (image == null)
                {
                    this.logger.Info($"Probably not valid map {file}");
                    continue;
                }

                MapFile? data = LoadMap(mapsLoader, file);
                if (data != null)
                {
                    yield return new DowMap()
                    {
                        Name = data.Name,
                        Details = data.Description,
                        Image = image,
                        Players = data.Players,
                        Size = data.Size
                    };
                }
            }
        }

        public IEnumerable<GameRuleFile> GetGameRules(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string rulesPath = Path.Combine(dowPath, module.ModFolder, "Data", "scar", "winconditions");

            var loader = new GameRuleLoader();
            return GetFiles(rulesPath, "*_local.lua", SearchOption.TopDirectoryOnly)
                .Select(file => loader.Load(file))
                .Where(rule => rule != null) as IEnumerable<GameRuleFile>;
        }

        public string? GetImage(ISet<string> images, string file)
        {
            string fileNoExt = Path.GetFileNameWithoutExtension(file);
            string? image = null;
            if (images.Contains(fileNoExt + "_icon_custom.tga"))
            {
                image = fileNoExt + "_icon_custom.tga";
            }
            else if (images.Contains(fileNoExt + "_icon.tga"))
            {
                image = fileNoExt + "_icon.tga";
            }
            return image;
        }

        private MapFile? LoadMap(MapLoader mapsLoader, string file)
        {
            try
            {
                return mapsLoader.Load(file);
            }
            catch (IOException ex)
            {
                this.logger.Info(ex, $"Failed to load {file}");
            }
            return null;
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
