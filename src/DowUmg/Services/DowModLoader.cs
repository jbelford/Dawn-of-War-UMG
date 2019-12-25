using DowUmg.Data.Entities;
using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DowUmg.Services
{
    public class DowModLoader : IEnableLogger
    {
        private readonly IFilePathProvider filePathProvider;
        private readonly ILogger logger;
        private readonly ModuleExtractorFactory moduleExtractorFactory = new ModuleExtractorFactory();

        public DowModLoader(IFilePathProvider? filePathProvider = null)
        {
            this.filePathProvider = filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
            this.logger = this.Log();
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
                        Locales = new LocaleStore(GetLocales(module.ModFolder).ToArray())
                    };
                }
            }

            foreach (UnloadedMod unloaded in modules.Values)
            {
                foreach (string name in unloaded.File.RequiredMods)
                {
                    if (modules.ContainsKey(name))
                    {
                        unloaded.Dependencies.Add(modules[name]);
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

            // Add unloaded mods for the uncompressed data possibly contained in vanilla mod folders
            // This data would include custom maps that were installed elsewhere
            foreach (var mod in modules.Values.Where(unloaded => unloaded.File.IsVanilla))
            {
                yield return new UnloadedMod()
                {
                    File = CreateAdditionsModule(mod.File),
                    Locales = mod.Locales,
                    Dependencies = mod.Dependencies
                };
            }
        }

        public DowMod LoadMod(UnloadedMod unloaded, Dictionary<string, UnloadedMod> allUnloaded, LoadMemo memo,
            LocaleStore? parentLocales = null)
        {
            DowMod? existing = memo.Get(unloaded.File.ModFolder, unloaded.File.IsVanilla);
            if (existing != null)
            {
                return existing;
            }

            LocaleStore newLocales = new LocaleStore();
            if (parentLocales != null)
            {
                newLocales.Dependencies.Add(parentLocales);
            }
            newLocales.Dependencies.Add(unloaded.Locales);

            var mod = new DowMod()
            {
                Name = unloaded.File.UIName,
                ModFolder = unloaded.File.ModFolder,
                Details = unloaded.File.Description,
                IsVanilla = unloaded.File.IsVanilla,
                Playable = unloaded.File.Playable
            };

            mod.Dependencies = new List<DowModDependency>();

            foreach (var dependency in unloaded.Dependencies)
            {
                mod.Dependencies.Add(new DowModDependency()
                {
                    MainMod = mod,
                    DepMod = LoadMod(allUnloaded[dependency.File.ModFolder], allUnloaded, memo, unloaded.Locales)
                });
            }

            using IModuleDataExtractor extractor = this.moduleExtractorFactory.Create(unloaded.File);

            mod.Races = new List<DowRace>();

            foreach (RaceFile race in extractor.GetRaces().Where(race => race.Playable))
            {
                mod.Races.Add(new DowRace()
                {
                    Name = newLocales.Replace(race.Name),
                    Description = newLocales.Replace(race.Description)
                });
            }

            mod.Maps = new List<DowMap>();

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
                    Name = newLocales.Replace(map.Name),
                    Details = newLocales.Replace(map.Description),
                    Players = map.Players,
                    Size = map.Size,
                    Image = image
                });
            }

            mod.Rules = new List<GameRule>();

            foreach (GameRuleFile rule in extractor.GetGameRules())
            {
                mod.Rules.Add(new GameRule()
                {
                    Name = newLocales.Replace(rule.Title),
                    Details = newLocales.Replace(rule.Description),
                    IsWinCondition = rule.VictoryCondition
                });
            }

            memo.Put(mod);

            return mod;
        }

        private static DowModuleFile CreateAdditionsModule(DowModuleFile mod)
        {
            return new DowModuleFile()
            {
                Description = "Custom uncompressed data found in the base game (Map Addons, etc.)",
                DllName = mod.DllName,
                ModFolder = mod.ModFolder,
                ModVersion = mod.ModVersion,
                Playable = mod.Playable,
                RequiredMods = mod.RequiredMods,
                UIName = $"{mod.UIName} - Additions",
                IsVanilla = false
            };
        }

        private IEnumerable<Locales> GetLocales(string modFolder)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string localePath = Path.Combine(dowPath, modFolder, "Locale", "English");

            try
            {
                string[] files = Directory.GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);
                var ucsLoader = new LocaleLoader();
                return files.Select(x => ucsLoader.Load(x));
            }
            catch (DirectoryNotFoundException)
            {
                return Enumerable.Empty<Locales>();
            }
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
        public List<UnloadedMod> Dependencies { get; set; } = new List<UnloadedMod>();
    }

    public class LoadMemo
    {
        private Dictionary<string, (DowMod? vanilla, DowMod? mod)> dict = new Dictionary<string, (DowMod? vanilla, DowMod? mod)>();

        public DowMod? Get(string modFolder, bool isVanilla)
        {
            if (this.dict.ContainsKey(modFolder))
            {
                var (vanilla, mod) = this.dict[modFolder];
                return isVanilla ? vanilla : mod;
            }

            return null;
        }

        public void Put(DowMod newMod)
        {
            (DowMod? vanilla, DowMod? mod) value = (null, null);
            if (this.dict.ContainsKey(newMod.ModFolder))
            {
                value = this.dict[newMod.ModFolder];
            }
            if (newMod.IsVanilla)
            {
                this.dict[newMod.ModFolder] = (newMod, value.mod);
            }
            else
            {
                this.dict[newMod.ModFolder] = (value.vanilla, newMod);
            }
        }
    }
}
