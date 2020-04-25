using DowUmg.Data.Entities;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class GameInfo
    {
        public GameOptions Options { get; set; } = null!;
        public List<GameRule> Rules { get; set; } = new List<GameRule>();
    }

    public class CampaignGameInfo : GameInfo
    {
        public List<CustomRule> CustomRules { get; set; } = new List<CustomRule>();
    }
}
