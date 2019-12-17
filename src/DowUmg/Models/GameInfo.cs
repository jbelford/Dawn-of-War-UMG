using DowUmg.Constants;
using DowUmg.Data.Entities;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class GameInfo
    {
        public GameDifficulty? Difficulty { get; set; }
        public GameResourceRate? ResourceRate { get; set; }
        public GameSpeed? Speed { get; set; }
        public GameStartResource? StartingResources { get; set; }

        public bool? RandomPosition { get; set; }
        public bool? RandomTeams { get; set; }
        public bool? Sharing { get; set; }

        public List<CustomRule> CustomRules { get; set; } = new List<CustomRule>();
        public List<GameRule> Rules { get; set; } = new List<GameRule>();
    }
}
