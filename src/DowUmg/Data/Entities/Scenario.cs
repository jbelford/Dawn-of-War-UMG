using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class Scenario
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public bool InheritedWinConditions { get; set; } = true;
        public bool InheritedGameRules { get; set; } = true;
        public bool InheritedCustomRules { get; set; } = true;
        public bool InheritedGameOptions { get; set; } = true;

        public Campaign Campaign { get; } = null!;
        public GameInfo Info { get; set; } = null!;
        public DowMap Map { get; set; } = null!;
        public List<ScenarioPlayers> Players { get; set; } = new List<ScenarioPlayers>();
    }
}
