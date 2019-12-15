using DowUmg.FileFormats;
using DowUmg.Interfaces;
using Splat;
using System.IO;

namespace DowUmg.Services
{
    internal class ModuleExtractorFactory
    {
        private readonly IFilePathProvider filePathProvider;

        internal ModuleExtractorFactory(IFilePathProvider? filePathProvider = null)
        {
            this.filePathProvider = filePathProvider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        internal IModuleDataExtractor Create(DowModuleFile file)
        {
            string folder = Path.Combine(filePathProvider.SoulstormLocation, file.ModFolder);

            switch (file.ModFolder.ToLower())
            {
                case "dxp2":
                case "w40k":
                    if (!file.UIName.Contains("Additions"))
                    {
                        string cacheFolder = Path.Combine(filePathProvider.AppDataLocation, file.ModFolder);
                        return new ModuleArchiveExtractor(Path.Combine(folder, $"{file.ModFolder}Data.sga"), cacheFolder);
                    }
                    // Yep this is the only time goto is OK
                    goto default;

                default:
                    return new ModuleFileSystemExtractor(folder);
            }
        }

        internal ModuleFileSystemExtractor CreateFileSystem(DowModuleFile file)
        {
            string folder = Path.Combine(filePathProvider.SoulstormLocation, file.ModFolder);
            return new ModuleFileSystemExtractor(folder);
        }
    }
}
