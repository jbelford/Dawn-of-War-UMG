using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public record Matchup(DowMap Map, GameInfo GameInfo, List<MatchupPlayer> Players);

    public record MatchupPlayer(string name, int team, MatchupPlayerRace? race = null)
    {
        public string Name { get; set; } = name;

        public int Team { get; set; } = team;

        public MatchupPlayerRace? Race { get; set; } = race;

        public int Position { get; set; }
    }

    public record MatchupPlayerRace(string Name, string FileName);
}
