using DowUmg.Services.Data.Entities;
using DowUmg.Services.Enums;
using System.Collections.Generic;

namespace DowUmg.Services.Models
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
        public List<GameInfoRule> Rules { get; set; } = new List<GameInfoRule>();
    }

    /// <summary>
    /// Must explicitly define entity for many-to-many relationship
    /// </summary>
    public class GameInfoRule
    {
        public int RuleId { get; set; }
        public GameRule Rule { get; set; } = null!;
        public int InfoId { get; set; }
        public GameInfo Info { get; set; } = null!;
    }
}
