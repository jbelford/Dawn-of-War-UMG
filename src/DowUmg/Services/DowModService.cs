using DowUmg.Data.Entities;
using DowUmg.FileFormats;
using DowUmg.Interfaces;
using DowUmg.Repositories;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DowUmg.Services
{
    public class DowModService : IEnableLogger
    {
        private readonly IFilePathProvider filePathProvider;
        private readonly ILogger logger;
        private readonly ModsRepository mods;
        private readonly ModuleExtractorFactory moduleExtractorFactory = new ModuleExtractorFactory();

        public DowModService(IFilePathProvider? filePathProvider = null)
        {
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
            var modules = new Dictionary<string, UnloadedMod>();
            foreach (DowModuleFile module in GetAllModules())
            {
                if (modules.ContainsKey(module.ModFolder))
                {
                    modules[module.ModFolder].File.UIName += $" / {module.UIName}";
                }
                else
                {
                    modules[module.ModFolder] = new UnloadedMod()
                    {
                        File = module,
                        Locales = GetLocales(module)
                    };
                }
            }

            foreach (UnloadedMod unloaded in modules.Values)
            {
                foreach (string name in unloaded.File.RequiredMods)
                {
                    if (modules.ContainsKey(name))
                    {
                        unloaded.Locales.Dependencies.Add(modules[name].Locales);
                    }
                }
            }

            foreach (var module in modules.Values)
            {
                if (module.Locales != null)
                {
                    module.File.UIName = module.Locales.Replace(module.File.UIName);
                    module.File.Description = module.Locales.Replace(module.File.Description);
                }

                yield return module;
            }

            // Yeah whatever

            var dxp2 = modules["DXP2"];
            yield return new UnloadedMod() { File = CreateAdditionsModule(dxp2.File), Locales = dxp2.Locales };

            var w40k = modules["W40k"];
            yield return new UnloadedMod() { File = CreateAdditionsModule(w40k.File), Locales = w40k.Locales };
        }

        public DowMod LoadMod(UnloadedMod unloaded)
        {
            var mod = new DowMod()
            {
                Name = unloaded.File.UIName,
                ModFolder = unloaded.File.ModFolder,
                Details = unloaded.File.Description,
                IsAddition = unloaded.File.UIName.Contains("Additions")
            };

            using IModuleDataExtractor extractor = this.moduleExtractorFactory.Create(unloaded.File);

            foreach (RaceFile race in extractor.GetRaces().Where(race => race.Playable))
            {
                mod.Races.Add(new DowRace()
                {
                    Name = unloaded.Locales.Replace(race.Name),
                    Description = unloaded.Locales.Replace(race.Description)
                });
            }

            foreach (MapFile map in extractor.GetMaps())
            {
                string? image = extractor.GetMapImage(map.FileName);
                if (image == null)
                {
                    this.logger.Write($"{mod.Name} Probably not valid map {map.FileName}", LogLevel.Info);
                    continue;
                }

                mod.Maps.Add(new DowMap()
                {
                    Name = unloaded.Locales.Replace(map.Name),
                    Details = unloaded.Locales.Replace(map.Description),
                    Players = map.Players,
                    Size = map.Size,
                    Image = image
                });
            }

            foreach (GameRuleFile rule in extractor.GetGameRules())
            {
                mod.Rules.Add(new GameRule()
                {
                    Name = unloaded.Locales.Replace(rule.Title),
                    Details = unloaded.Locales.Replace(rule.Description),
                    IsWinCondition = rule.VictoryCondition
                });
            }

            return this.mods.Upsert(mod);
        }

        private static DowModuleFile CreateAdditionsModule(DowModuleFile mod)
        {
            return new DowModuleFile()
            {
                Description = mod.Description,
                DllName = mod.Description,
                ModFolder = mod.ModFolder,
                ModVersion = mod.ModVersion,
                Playable = mod.Playable,
                RequiredMods = mod.RequiredMods,
                UIName = $"{mod.UIName} - Additions"
            };
        }

        private LocaleStore GetLocales(DowModuleFile module)
        {
            using var extractor = this.moduleExtractorFactory.CreateFileSystem(module);
            return new LocaleStore(extractor.GetLocales().ToArray());
        }

        private IEnumerable<DowModuleFile> GetAllModules()
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            var moduleLoader = new ModuleLoader();
            return GetFiles(dowPath, "*.module", SearchOption.TopDirectoryOnly).Select(file => moduleLoader.Load(file));
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

    public class UnloadedMod
    {
        public DowModuleFile File { get; set; } = null!;
        public LocaleStore Locales { get; set; } = null!;
    }
}
