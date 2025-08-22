using System.IO;
using System.Linq;
using DowUmg.FileFormats;
using DowUmg.Platform;
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
                var dataSgaName = $"{file.ModFolder}Data.sga";
                if (file.ArchiveFiles.Length > 0)
                {
                    for (int i = 0; i < file.ArchiveFiles.Length; i++)
                    {
                        if (!file.ArchiveFiles[i].Contains("%locale%") && !file.ArchiveFiles[i].Contains("-new"))
                        {
                            dataSgaName = file.ArchiveFiles[i];
                            break;
                        }
                    }
                }

                return new ModuleArchiveExtractor(
                    Path.Combine(folder, dataSgaName),
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
