using DowUmg.Interfaces;
using NLua;
using System.IO;

namespace DowUmg.FileFormats
{
    public class GameRuleFile
    {
        public string Title { get; set; } = null!;
        public string WinMessage { get; set; } = null!;
        public string LoseMessage { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Exclusive { get; set; }
        public bool VictoryCondition { get; set; }
        public bool AlwaysOn { get; set; }
    }

    public class GameRuleLoader : IFileLoader<GameRuleFile?>
    {
        public GameRuleFile? Load(string filePath)
        {
            return Load(File.OpenRead(filePath));
        }

        public GameRuleFile? Load(Stream stream)
        {
            using var lua = new Lua();
            using (var r = new StreamReader(stream))
            {
                lua.DoString(r.ReadToEnd());
            }

            if (lua["Localization"] is LuaTable table)
            {
                return new GameRuleFile()
                {
                    Title = (string)table["title"],
                    WinMessage = (string)table["win_message"],
                    LoseMessage = (string)table["lose_message"],
                    Description = (string)table["description"],
                    VictoryCondition = (bool)table["victory_condition"],
                    AlwaysOn = (bool)table["always_on"],
                    Exclusive = (bool)table["exclusive"]
                };
            }

            return null;
        }
    }
}
