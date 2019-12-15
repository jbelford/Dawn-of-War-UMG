using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DowUmg.Services
{
    public class ModuleFileSystemExtractor : IModuleDataExtractor, IEnableLogger
    {
        private readonly string rootDir;
        private readonly ILogger logger;
        private readonly string mapsPath;
        private readonly Lazy<ISet<string>> images;

        public ModuleFileSystemExtractor(string rootDir)
        {
            this.rootDir = rootDir;
            logger = this.Log();
            mapsPath = Path.Combine(this.rootDir, "Data", "Scenarios", "mp");

            images = new Lazy<ISet<string>>(
                () => GetFiles(mapsPath, "*.tga", SearchOption.TopDirectoryOnly)
                    .Select(x => Path.GetFileName(x).ToLower())
                    .ToHashSet());
        }

        public void Dispose()
        {
        }

        public IEnumerable<GameRuleFile> GetGameRules()
        {
            string rulesPath = Path.Combine(rootDir, "Data", "scar", "winconditions");

            var loader = new GameRuleLoader();
            return GetFiles(rulesPath, "*_local.lua", SearchOption.TopDirectoryOnly)
                .Select(file => loader.Load(file))
                .Where(rule => rule != null) as IEnumerable<GameRuleFile>;
        }

        public IEnumerable<Locales> GetLocales()
        {
            string localePath = Path.Combine(rootDir, "Locale", "English");

            string[] files = GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);

            var ucsLoader = new LocaleLoader();
            return files.Select(x => ucsLoader.Load(x));
        }

        public IEnumerable<MapFile> GetMaps()
        {
            var mapsLoader = new MapLoader();
            foreach (string file in GetFiles(mapsPath, "*.sgb", SearchOption.TopDirectoryOnly))
            {
                MapFile? mapFile = LoadMap(mapsLoader, file);
                if (mapFile != null)
                {
                    yield return mapFile;
                }
            }
        }

        public string? GetMapImage(string fileName)
        {
            string fileNoExt = Path.GetFileNameWithoutExtension(fileName).ToLower();
            string? image = null;
            if (images.Value.Contains(fileNoExt + "_mm_custom.tga"))
            {
                image = fileNoExt + "_mm_custom.tga";
            }
            else if (images.Value.Contains(fileNoExt + "_mm.tga"))
            {
                image = fileNoExt + "_mm.tga";
            }
            else if (images.Value.Contains(fileNoExt + "_icon_custom.tga"))
            {
                image = fileNoExt + "_icon_custom.tga";
            }
            else if (images.Value.Contains(fileNoExt + "_icon.tga"))
            {
                image = fileNoExt + "_icon.tga";
            }
            return image;
        }

        private MapFile? LoadMap(MapLoader mapsLoader, string file)
        {
            MapFile? mapFile = null;
            try
            {
                mapFile = mapsLoader.Load(file);
            }
            catch (IOException ex)
            {
                logger.Write(ex, $"Failed to load {file}", LogLevel.Error);
            }

            return mapFile;
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
