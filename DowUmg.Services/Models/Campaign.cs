using System.Collections.Generic;

namespace DowUmg.Services.Models
{
    public class Campaign : InfoObject
    {
        public GameInfo Info { get; } = new GameInfo();
        public List<Scenario> Scenarios { get; } = new List<Scenario>();
        public List<Army> Armies { get; } = new List<Army>();
        public List<Alliance> Alliances { get; } = new List<Alliance>();
    }
}
