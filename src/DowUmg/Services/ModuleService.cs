using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
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
        public IObservable<DowModuleFile> GetAllModules()
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            return Observable.Create<DowModuleFile>(observer =>
                {
                    string[] files = Directory.GetFiles(dowPath, "*.module", SearchOption.TopDirectoryOnly);

                    var moduleLoader = new ModuleLoader();

                    foreach (string file in files)
                    {
                        observer.OnNext(moduleLoader.Load(file));
                    }

                    observer.OnCompleted();

                    return Disposable.Empty;
                });
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

        public IObservable<MapFile> GetMaps(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            return Observable.Create<MapFile>((observer) =>
                {
                    string mapsPath = Path.Combine(dowPath, module.ModFolder, "Data", "Scenarios", "mp");

                    string[] files = Directory.GetFiles(mapsPath, "*.sgb", SearchOption.TopDirectoryOnly);

                    var mapsLoader = new MapLoader();

                    foreach (string file in files)
                    {
                        observer.OnNext(mapsLoader.Load(file));
                    }

                    observer.OnCompleted();

                    return Disposable.Empty;
                });
        }
    }
}
