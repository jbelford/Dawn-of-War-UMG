﻿using DowUmg.Data.Entities;
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

        internal DowModService(IFilePathProvider? filePathProvider = null, ModsRepository? modsRepository = null)
        {
            this.filePathProvider = filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
            this.mods = modsRepository ?? Locator.Current.GetService<ModsRepository>();
            this.logger = this.Log();
        }

        public List<DowMod> GetLoadedMods()
        {
            List<DowMod> mods = this.mods.GetAll();
            mods.Sort((a, b) => a.Name.CompareTo(b.Name));
            return mods;
        }

        public void RemoveLoadedMods()
        {
            this.mods.DropAll();
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
                yield return new UnloadedMod() { File = CreateAdditionsModule(mod.File), Locales = mod.Locales };
            }
        }

        public DowMod LoadMod(UnloadedMod unloaded, Dictionary<string, UnloadedMod> allUnloaded, LoadMemo memo)
        {
            DowMod? existing = memo.Get(unloaded.File.ModFolder, unloaded.File.IsVanilla);
            if (existing != null)
            {
                return existing;
            }

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
                    DepMod = LoadMod(allUnloaded[dependency.File.ModFolder], allUnloaded, memo)
                });
            }

            using IModuleDataExtractor extractor = this.moduleExtractorFactory.Create(unloaded.File);

            mod.Races = new List<DowRace>();

            foreach (RaceFile race in extractor.GetRaces().Where(race => race.Playable))
            {
                mod.Races.Add(new DowRace()
                {
                    Name = unloaded.Locales.Replace(race.Name),
                    Description = unloaded.Locales.Replace(race.Description)
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
                    Name = unloaded.Locales.Replace(map.Name),
                    Details = unloaded.Locales.Replace(map.Description),
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
                    Name = unloaded.Locales.Replace(rule.Title),
                    Details = unloaded.Locales.Replace(rule.Description),
                    IsWinCondition = rule.VictoryCondition
                });
            }

            var newMod = this.mods.Upsert(mod);

            memo.Put(newMod);

            return newMod;
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
