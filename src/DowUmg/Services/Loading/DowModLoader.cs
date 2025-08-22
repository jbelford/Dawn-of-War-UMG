using System.Collections.Generic;
using System.IO;
using System.Linq;
using DowUmg.Data.Entities;
using DowUmg.FileFormats;
using DowUmg.Platform;
using Splat;

namespace DowUmg.Services
{
    public class UnloadedMod
    {
        public DowModuleFile File { get; set; } = null!;
        public LocaleStore Locales { get; set; } = null!;
        public List<UnloadedMod> Dependencies { get; set; } = new List<UnloadedMod>();
    }

    public class DowModLoader : IEnableLogger
    {
        private readonly IFilePathProvider filePathProvider;
        private readonly ILogger logger;
        private readonly ModuleExtractorFactory moduleExtractorFactory =
            new ModuleExtractorFactory();

        public DowModLoader(IFilePathProvider? filePathProvider = null)
        {
            this.filePathProvider =
                filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
            this.logger = this.Log();
        }

        public string GetMapImagePath(DowMap map)
        {
            string subPath = Path.Combine(map.Mod.ModFolder, "data", "scenarios", "mp", map.Image);
            return File.Exists(Path.Combine(filePathProvider.SoulstormLocation, subPath))
                ? Path.Combine(filePathProvider.SoulstormLocation, subPath)
                : Path.Combine(filePathProvider.ModCacheLocation, subPath);
        }

        public IEnumerable<UnloadedMod> GetUnloadedMods()
        {
            Dictionary<string, DowModuleFile> modFiles = GetAllModules()
                .ToDictionary(m => m.FileName, m => m);

            string dowPath = this.filePathProvider.SoulstormLocation;
            LocaleStore baseLocales = new(GetLocales(Path.Combine(dowPath, "Engine")).ToArray());

            // Mod data is stored in folders. Different mods may share the same mod folder for locales
            Dictionary<string, LocaleStore> modFolders = modFiles
                .Values.Select(m => m.ModFolder)
                .Distinct()
                .ToDictionary(
                    folder => folder,
                    folder => new LocaleStore(GetLocales(folder).ToArray()) { Dependencies = [baseLocales] }
                );

            // Create dictionary of unloaded mods by filename
            Dictionary<string, UnloadedMod> unloadedMods = modFiles
                .Values.Select(moduleFile => new UnloadedMod()
                {
                    File = moduleFile,
                    Locales = modFolders[moduleFile.ModFolder]
                })
                .ToDictionary(m => m.File.FileName, m => m);

            // Add locale dependencies
            foreach (UnloadedMod unloaded in unloadedMods.Values)
            {
                foreach (string name in unloaded.File.RequiredMods)
                {
                    if (unloadedMods.TryGetValue(name, out UnloadedMod? value))
                    {
                        unloaded.Dependencies.Add(value);
                        unloaded.Locales.Dependencies.Add(value.Locales);
                    }
                }
            }

            // Replace names with locales once locale dependency graph is setup
            foreach (var module in unloadedMods.Values)
            {
                module.File.UIName = module.Locales.Replace(module.File.UIName);
                module.File.Description = module.Locales.Replace(module.File.Description);

                yield return module;
            }

            // Add unloaded mods for the uncompressed data possibly contained in vanilla mod folders
            // This data would include custom maps that were installed elsewhere
            foreach (var mod in unloadedMods.Values.Where(unloaded => unloaded.File.IsVanilla))
            {
                yield return new UnloadedMod()
                {
                    File = CreateAdditionsModule(mod.File),
                    Locales = mod.Locales,
                    Dependencies = mod.Dependencies
                };
            }
        }

        public DowMod LoadMod(
            UnloadedMod unloaded,
            Dictionary<(string, bool), UnloadedMod> allUnloaded,
            LoadMemo memo,
            LocaleStore? parentLocales = null
        )
        {
            DowMod? existing = memo.GetMod(unloaded.File);
            if (existing != null)
            {
                return existing;
            }

            var newLocales = new LocaleStore();
            if (parentLocales != null)
            {
                newLocales.Dependencies.Add(parentLocales);
            }
            newLocales.Dependencies.Add(unloaded.Locales);

            var mod = new DowMod
            {
                Name = unloaded.File.UIName,
                ModFile = unloaded.File.FileName,
                Details = unloaded.File.Description,
                IsVanilla = unloaded.File.IsVanilla,
                Playable = unloaded.File.Playable,
                Dependencies = unloaded
                    .Dependencies.Select(dep =>
                        LoadMod(
                            allUnloaded[(dep.File.FileName, dep.File.IsVanilla)],
                            allUnloaded,
                            memo,
                            newLocales
                        )
                    )
                    .ToList(),
            };

            memo.PutMod(mod);

            DowModData? existingData = memo.GetData(unloaded.File);
            if (existingData != null)
            {
                mod.Data = existingData;
                existingData.Mods.Add(mod);
                return mod;
            }

            using IModuleDataExtractor dataExtractor = this.moduleExtractorFactory.Create(
                unloaded.File
            );

            var races = dataExtractor
                .GetRaces()
                .Where(race => race.Playable)
                .Select(race => new DowRace()
                {
                    Name = newLocales.Replace(race.Name).Trim(),
                    Description = newLocales.Replace(race.Description),
                    FileName = race.FileName,
                })
                .ToList();

            var maps = new List<DowMap>();
            foreach (MapFile map in dataExtractor.GetMaps())
            {
                if (map.Players < 2 || map.Players > 8)
                {
                    this.logger.Write(
                        $"({mod.Name}) Probably not a valid map {map.FileName} as it does not contain a valid player size: '{map.Players}'",
                        LogLevel.Info
                    );
                    continue;
                }

                string? image = dataExtractor.GetMapImage(map.FileName);
                if (image == null)
                {
                    this.logger.Write(
                        $"({mod.Name}) Probably not valid map {map.FileName} as it does not have an image",
                        LogLevel.Info
                    );
                    continue;
                }

                maps.Add(
                    new DowMap()
                    {
                        Name = newLocales.Replace(map.Name),
                        Details = newLocales.Replace(map.Description),
                        Players = map.Players,
                        Size = map.Size,
                        Image = image
                    }
                );
            }

            var rules = dataExtractor
                .GetGameRules()
                .Select(rule => new GameRule()
                {
                    Name = newLocales.Replace(rule.Title),
                    Details = newLocales.Replace(rule.Description),
                    FileName = rule.FileName,
                    IsWinCondition = rule.VictoryCondition
                })
                .ToList();

            mod.Data = new DowModData()
            {
                ModFolder = unloaded.File.ModFolder,
                Mods = new List<DowMod>([mod]),
                Races = races,
                Maps = maps,
                Rules = rules
            };

            memo.PutData(mod.Data, mod.IsVanilla);

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
                Playable = false,
                ArchiveFiles = [],
                RequiredMods = mod.RequiredMods,
                UIName = $"{mod.UIName} - Additions",
                IsVanilla = false,
                FileName = mod.FileName,
            };
        }

        private IEnumerable<Locales> GetLocales(string modFolder)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string localePath = Path.Combine(dowPath, modFolder, "Locale", "English");
            string subPath = Path.Combine(dowPath, modFolder, "Locale", "English_Chinese");

            try
            {
                IEnumerable<string> files = GetUcsFiles(localePath);
                if (Directory.Exists(subPath))
                {
                    files = files.Concat(GetUcsFiles(subPath));
                }

                var ucsLoader = new LocaleLoader();
                return files.Select(ucsLoader.Load);
            }
            catch (DirectoryNotFoundException)
            {
                return [];
            }
        }

        private static IEnumerable<string> GetUcsFiles(string localePath)
        {
            return Directory.GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);
        }

        private IEnumerable<DowModuleFile> GetAllModules()
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            var moduleLoader = new ModuleLoader();
            return GetFiles(dowPath, "*.module", SearchOption.TopDirectoryOnly)
                .Select(file => moduleLoader.Load(file));
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
