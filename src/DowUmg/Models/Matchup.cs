using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public class Matchup(DowMap map, GameInfo info)
    {
        public List<List<MatchupArmy>> Teams { get; } = new List<List<MatchupArmy>>();
        public DowMap Map { get; set; } = map;
        public GameInfo GameInfo { get; set; } = info;
    }

    public class MatchupTeam
    {
        public MatchupTeam(string name)
        {
            Name = name;
        }

        public List<MatchupArmy> Members { get; } = new List<MatchupArmy>();

        public string Name { get; }
    }

    public class MatchupArmy
    {
        public MatchupArmy(string race, int position)
        {
            Race = race;
            Position = position;
        }

        public string Race { get; }
        public int Position { get; }
    }
}
