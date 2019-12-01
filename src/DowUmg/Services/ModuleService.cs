using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System;
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
                    string[] files = GetFiles(dowPath, "*.module", SearchOption.TopDirectoryOnly);

                    var moduleLoader = new ModuleLoader();

                    foreach (string file in files)
                    {
                        observer.OnNext(moduleLoader.Load(file));
                    }

                    observer.OnCompleted();

                    return Disposable.Empty;
                });
        }

        public Locales? GetLocales(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            string localePath = Path.Combine(dowPath, module.ModFolder, "Locale", "English");

            string[] files = GetFiles(localePath, "*.ucs", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                return null;
            }

            var ucsLoader = new UcsLoader();
            return files.Select(x => ucsLoader.Load(x))
                .Aggregate((a, b) => a.Concat(b));
        }

        public IObservable<MapFile> GetMaps(DowModuleFile module)
        {
            string dowPath = this.filePathProvider.SoulstormLocation;
            return Observable.Create<MapFile>((observer) =>
                {
                    string mapsPath = Path.Combine(dowPath, module.ModFolder, "Data", "Scenarios", "mp");

                    string[] files = GetFiles(mapsPath, "*.sgb", SearchOption.TopDirectoryOnly);

                    var mapsLoader = new MapLoader();

                    foreach (string file in files)
                    {
                        observer.OnNext(mapsLoader.Load(file));
                    }

                    observer.OnCompleted();

                    return Disposable.Empty;
                });
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
