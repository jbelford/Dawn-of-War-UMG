using DowUmg.Constants;
using DowUmg.Interfaces;
using IniParser;
using IniParser.Model;
using System.Linq;
using System.Text.RegularExpressions;

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

            var reg = new Regex(@"^RequiredMod\.\d+$");
            string modFolder = global["ModFolder"];

            return new DowModuleFile()
            {
                UIName = global["UIName"],
                Description = global["Description"],
                DllName = global["DllName"],
                Playable = "1".Equals(global["Playable"]),
                ModFolder = modFolder,
                ModVersion = global["ModVersion"],
                RequiredMods = global.Where(x => reg.IsMatch(x.KeyName)).Select(x => x.Value).ToArray(),
                IsVanilla = IsVanilla(modFolder)
            };
        }

        private static bool IsVanilla(string str) =>
            DowConstants.DXP2Folder.Equals(str, System.StringComparison.OrdinalIgnoreCase)
            || DowConstants.W40kFolder.Equals(str, System.StringComparison.OrdinalIgnoreCase);
    }
}
