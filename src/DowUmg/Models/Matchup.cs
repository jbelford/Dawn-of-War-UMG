using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public record Matchup(DowMap Map, GameInfo GameInfo, List<MatchupPlayer> Players);

    public record MatchupPlayer(string Name, int Team, string? Race = null);
}
