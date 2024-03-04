using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public record Matchup(DowMap Map, GameInfo GameInfo, List<MatchupPlayer> Players);

    public class MatchupPlayer(string name, int team, string? race = null)
    {
        public string Name { get; set; } = name;

        public int Team { get; set; } = team;

        public string? Race { get; set; } = race;

        public int Position { get; set; }
    }
}
