using System.Collections.Generic;

namespace DowUmg.Services.Models
{
    public class GameInfo
    {
        public List<string> CustomRules { get; } = new List<string>();
        public List<GameRule> Rules { get; } = new List<GameRule>();
        public GameOptions Options { get; }
    }
}
