using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class Campaign
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public bool IsSingle { get; set; }
        public GameInfo Info { get; } = new GameInfo();
        public List<Army> Armies { get; } = new List<Army>();
        public List<Alliance> Alliances { get; } = new List<Alliance>();
        public List<SaveGame> SaveGames { get; } = new List<SaveGame>();
        public List<Scenario> Scenarios { get; } = new List<Scenario>();
    }
}
