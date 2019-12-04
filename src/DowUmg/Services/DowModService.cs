using DowUmg.Data.Entities;
using DowUmg.FileFormats;
using DowUmg.Interfaces;
using DowUmg.Repositories;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace DowUmg.Services
{
    public class DowModService : IEnableLogger
    {
        private readonly ModuleService moduleService;
        private readonly IFilePathProvider filePathProvider;
        private readonly IFullLogger logger;
        private readonly ModsRepository mods;

        public DowModService(ModuleService? moduleService = null, IFilePathProvider? filePathProvider = null)
        {
            this.moduleService = moduleService ?? Locator.Current.GetService<ModuleService>();
            this.filePathProvider = filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
            this.mods = new ModsRepository();
            this.logger = this.Log();
        }

        public List<DowMod> GetLoadedMods()
        {
            return this.mods.GetAll();
        }

        public IEnumerable<UnloadedMod> GetUnloadedMods()
        {
            Dictionary<string, UnloadedMod> mods = this.moduleService.GetAllModules()
                .Select(module => new UnloadedMod() { File = module, Locales = this.moduleService.GetLocales(module) })
                .GroupBy(mod => mod.File.ModFolder)
                .ToDictionary(g => g.Key, g =>
                {
                    UnloadedMod last = g.Last();
                    last.File.UIName = g.Select(mod => mod.File.UIName).Aggregate((a, b) => $"{a} / {b}");
                    return last;
                });

            foreach (UnloadedMod mod in mods.Values)
            {
                foreach (string name in mod.File.RequiredMods)
                {
                    if (mods.ContainsKey(name))
                    {
                        mod.Locales.Dependencies.Add(mods[name].Locales);
                    }
                }
            }

            return mods.Values.Select(module =>
                {
                    if (module.Locales != null)
                    {
                        module.File.UIName = module.Locales.Replace(module.File.UIName);
                        module.File.Description = module.Locales.Replace(module.File.Description);
                    }
                    return module;
                });
        }

        public DowMod LoadMod(UnloadedMod unloaded)
        {
            var mod = new DowMod()
            {
                Name = unloaded.File.UIName,
                ModFolder = unloaded.File.ModFolder,
                Details = unloaded.File.Description,
            };

            mod.Maps.AddRange(this.moduleService.GetMaps(unloaded.File)
                .Select(map =>
                {
                    map.Name = unloaded.Locales.Replace(map.Name);
                    map.Details = unloaded.Locales.Replace(map.Details);
                    return map;
                }));

            mod.Rules.AddRange(this.moduleService.GetGameRules(unloaded.File)
                .Select(rule => new GameRule()
                {
                    Name = unloaded.Locales.Replace(rule.Title),
                    Details = unloaded.Locales.Replace(rule.Description),
                    IsWinCondition = rule.VictoryCondition
                }));

            return this.mods.Upsert(mod);
        }

        public DowMod LoadModArchive(UnloadedMod unloaded)
        {
            string archiveFolder = Path.Combine(this.filePathProvider.AppDataLocation, unloaded.File.ModFolder, "data", "scenarios", "mp");
            if (Directory.Exists(archiveFolder))
            {
                Directory.Delete(archiveFolder, true);
            }

            Directory.CreateDirectory(archiveFolder);

            string sgaFileName = $"{unloaded.File.ModFolder}Data.sga";

            var mod = new DowMod()
            {
                Name = unloaded.File.UIName,
                ModFolder = unloaded.File.ModFolder,
                Details = unloaded.File.Description,
            };

            var images = new HashSet<string>();

            using var sgaReader = new SgaFileReader(Path.Combine(this.filePathProvider.SoulstormLocation, unloaded.File.ModFolder, sgaFileName));

            foreach (var rawFile in sgaReader.GetScenarioImages())
            {
                images.Add(rawFile.Name);
                File.WriteAllBytes(Path.Combine(archiveFolder, rawFile.Name), rawFile.Data);
            }

            var mapLoader = new MapLoader();
            foreach (var scenario in sgaReader.GetScenarios())
            {
                string? image = this.moduleService.GetImage(images, scenario.Name);
                if (image == null)
                {
                    this.logger.Info($"Probably not valid map {scenario.Name}");
                    continue;
                }

                MapFile? mapFile = LoadMap(mapLoader, scenario);
                if (mapFile != null)
                {
                    mod.Maps.Add(new DowMap()
                    {
                        Name = unloaded.Locales.Replace(mapFile.Name),
                        Details = unloaded.Locales.Replace(mapFile.Description),
                        Image = image,
                        Players = mapFile.Players,
                        Size = mapFile.Size
                    });
                }
            }

            var gameRuleLoader = new GameRuleLoader();
            foreach (var winCondtion in sgaReader.GetWinConditions())
            {
                GameRuleFile? gameRule = gameRuleLoader.Load(new MemoryStream(winCondtion.Data));
                if (gameRule != null)
                {
                    mod.Rules.Add(new GameRule()
                    {
                        Name = unloaded.Locales.Replace(gameRule.Title),
                        Details = unloaded.Locales.Replace(gameRule.Description),
                        IsWinCondition = gameRule.VictoryCondition
                    });
                }
            }

            return this.mods.Upsert(mod);
        }

        public MapFile? LoadMap(MapLoader loader, SgaRawFile file)
        {
            try
            {
                return loader.Load(new MemoryStream(file.Data));
            }
            catch (IOException ex)
            {
                this.logger.Info(ex, $"Failed to load {file.Name}");
            }
            return null;
        }
    }

    public class UnloadedMod
    {
        public DowModuleFile File { get; set; } = null!;
        public LocaleStore Locales { get; set; } = null!;
    }
}
