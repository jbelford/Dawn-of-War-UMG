using System.IO;
using System.Linq;
using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;

namespace DowUmg.Services
{
    internal class ModuleExtractorFactory
    {
        private readonly IFilePathProvider filePathProvider;

        internal ModuleExtractorFactory(IFilePathProvider? filePathProvider = null)
        {
            this.filePathProvider =
                filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        internal IModuleDataExtractor Create(DowModuleFile file)
        {
            string folder = Path.Combine(filePathProvider.SoulstormLocation, file.ModFolder);
            string cacheFolder = Path.Combine(filePathProvider.ModCacheLocation, file.ModFolder);

            if (file.IsVanilla)
            {
                return new ModuleArchiveExtractor(
                    Path.Combine(folder, $"{file.ModFolder}Data.sga"),
                    cacheFolder
                );
            }

            return new ModuleFileSystemExtractor(folder)
            {
                ArchiveExtractors = file.ArchiveFiles.Select(
                    (archive, index) =>
                        new ModuleArchiveExtractor(Path.Combine(folder, archive), cacheFolder)
                )
            };
        }
    }
}
