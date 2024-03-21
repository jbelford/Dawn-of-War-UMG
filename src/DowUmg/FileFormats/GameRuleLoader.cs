using System.IO;
using NLua;

namespace DowUmg.FileFormats
{
    public record GameRuleFile(
        string FileName,
        string Title,
        string WinMessage,
        string LoseMessage,
        string Description,
        bool Exclusive,
        bool VictoryCondition,
        bool AlwaysOn
    );

    internal class GameRuleLoader : IFileLoader<GameRuleFile?>
    {
        public GameRuleFile? Load(string filePath)
        {
            return Load(Path.GetFileName(filePath), File.OpenRead(filePath));
        }

        public GameRuleFile? Load(string fileName, Stream stream)
        {
            using var lua = new Lua();
            using (var r = new StreamReader(stream))
            {
                lua.DoString(r.ReadToEnd());
            }

            if (lua["Localization"] is LuaTable table)
            {
                return new GameRuleFile(
                    fileName,
                    (string)table["title"],
                    (string)table["win_message"],
                    (string)table["lose_message"],
                    (string)table["description"],
                    (bool)table["exclusive"],
                    (bool)table["victory_condition"],
                    (bool)table["always_on"]
                );
            }

            return null;
        }
    }
}
