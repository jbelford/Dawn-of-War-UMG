using DowUmg.Data.Entities;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class Matchup
    {
        public Matchup(DowMap map, GameInfo info)
        {
            Map = map;
            GameInfo = info;
        }

        public List<List<MatchupArmy>> Teams { get; } = new List<List<MatchupArmy>>();
        public DowMap Map { get; set; }
        public GameInfo GameInfo { get; set; }
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
