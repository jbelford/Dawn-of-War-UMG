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
    }

    public class ModuleLoader : IFileLoader<DowModuleFile>
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

            return new DowModuleFile()
            {
                UIName = global["UIName"],
                Description = global["Description"],
                DllName = global["DllName"],
                Playable = "1".Equals(global["Playable"]),
                ModFolder = global["ModFolder"],
                ModVersion = global["ModVersion"],
                RequiredMods = global.Where(x => reg.IsMatch(x.KeyName)).Select(x => x.Value).ToArray()
            };
        }
    }
}
