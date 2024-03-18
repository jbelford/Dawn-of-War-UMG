using System.Collections.Generic;
using System.IO;
using System.Linq;
using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;

namespace DowUmg.Services
{
    internal class ModuleArchiveExtractor : IModuleDataExtractor, IEnableLogger
    {
        private readonly SgaFileReader sgaFileReader;

        private readonly string cacheFolder;
        private readonly ILogger logger;

        public ModuleArchiveExtractor(string filePath, string cacheFolder)
        {
            this.sgaFileReader = new SgaFileReader(filePath);
            this.cacheFolder = cacheFolder;
            this.logger = this.Log();
        }

        public void Dispose()
        {
            this.sgaFileReader.Dispose();
        }

        public IEnumerable<GameRuleFile> GetGameRules()
        {
            var gameRuleLoader = new GameRuleLoader();
            foreach (var wincondition in sgaFileReader.GetWinConditions())
            {
                GameRuleFile? gameRule = gameRuleLoader.Load(new MemoryStream(wincondition.Data));
                if (gameRule != null)
                {
                    yield return gameRule;
                }
            }
        }

        public IEnumerable<MapFile> GetMaps()
        {
            var mapLoader = new MapLoader();
            foreach (var scenario in sgaFileReader.GetScenarios())
            {
                MapFile? mapFile = LoadMap(mapLoader, scenario);
                if (mapFile != null)
                {
                    yield return mapFile;
                }
            }
        }

        public string? GetMapImage(string fileName)
        {
            List<SgaRawFile> images = sgaFileReader.GetScenarioImage(fileName).ToList();
            if (images.Count == 0)
            {
                return null;
            }

            images.Sort((a, b) => a.Name.CompareTo(b.Name));

            SgaRawFile image = images.First();

            string imagesFolder = Path.Combine(cacheFolder, "data", "scenarios", "mp");
            Directory.CreateDirectory(imagesFolder);
            File.WriteAllBytes(Path.Combine(imagesFolder, image.Name), image.Data);

            return image.Name;
        }

        public IEnumerable<RaceFile> GetRaces()
        {
            var raceLoader = new RaceLoader();
            return sgaFileReader
                .GetRaces()
                .Select(file => raceLoader.Load(file.Name, new MemoryStream(file.Data)));
        }

        private MapFile? LoadMap(MapLoader mapLoader, SgaRawFile scenario)
        {
            MapFile? mapFile = null;
            try
            {
                mapFile = mapLoader.Load(new MemoryStream(scenario.Data), scenario.Name);
            }
            catch (IOException ex)
            {
                logger.Write(ex, $"Failed to load {scenario.Name}", LogLevel.Warn);
            }

            return mapFile;
        }
    }
}
