using System.Linq;
using System.Text.RegularExpressions;
using DowUmg.Constants;
using DowUmg.Interfaces;
using IniParser;
using IniParser.Model;

namespace DowUmg.FileFormats
{
    public class DowModuleFile
    {
        public string UIName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DllName { get; set; } = null!;
        public bool Playable { get; set; }
        public string ModFolder { get; set; } = null!;
        public string ModVersion { get; set; } = null!;
        public string[] ArchiveFiles { get; set; } = null!;
        public string[] RequiredMods { get; set; } = null!;
        public bool IsVanilla { get; set; }
    }

    internal class ModuleLoader : IFileLoader<DowModuleFile>
    {
        private readonly FileIniDataParser parser;

        public ModuleLoader()
        {
            this.parser = new FileIniDataParser();
            this.parser.Parser.Configuration.SkipInvalidLines = true;
        }

        public DowModuleFile Load(string filePath)
        {
            IniData data = this.parser.ReadFile(filePath);
            KeyDataCollection global = data["global"];

            var archiveFileReg = new Regex(@"^ArchiveFile\.\d+$");
            var requireModReg = new Regex(@"^RequiredMod\.\d+$");
            string modFolder = global["ModFolder"];

            return new DowModuleFile()
            {
                UIName = global["UIName"],
                Description = global["Description"],
                DllName = global["DllName"],
                Playable = "1".Equals(global["Playable"]),
                ModFolder = modFolder.ToLower(),
                ModVersion = global["ModVersion"],
                ArchiveFiles = global
                    .Where(x => archiveFileReg.IsMatch(x.KeyName))
                    .Select(x => x.Value.ToLower())
                    .ToArray(),
                RequiredMods = global
                    .Where(x => requireModReg.IsMatch(x.KeyName))
                    .Select(x => x.Value.ToLower())
                    .ToArray(),
                IsVanilla = DowConstants.IsVanilla(modFolder.ToLower())
            };
        }
    }
}
