using DowUmg.Data.Entities;
using DowUmg.Enums;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class GameInfo
    {
        public int Id { get; set; }

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
