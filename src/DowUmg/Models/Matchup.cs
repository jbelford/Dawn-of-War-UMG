using DowUmg.Data.Entities;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class Matchup
    {
        public Matchup(int teams)
        {
            Teams = new List<MatchupArmy>[teams];
        }

        public List<MatchupArmy>[] Teams { get; }
        public DowMap Map { get; set; } = null!;
        public GameOptions Options { get; set; } = null!;
        public List<GameRule> Rules { get; set; } = new List<GameRule>();
    }

    public class MatchupArmy
    {
        public string Race { get; set; }
    }
}
