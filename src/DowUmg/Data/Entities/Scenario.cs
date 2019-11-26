using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DowUmg.Data.Entities
{
    public class Scenario
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;

        public DowMap Map { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;

        public bool InheritedWinConditions { get; set; } = true;
        public bool InheritedGameRules { get; set; } = true;
        public bool InheritedCustomRules { get; set; } = true;
        public bool InheritedGameOptions { get; set; } = true;

        public string? Image { get; set; }

        public int? PreviousId { get; set; }

        [ForeignKey("PreviousId")]
        public Scenario? Previous { get; set; }

        public int? NextId { get; set; }

        [ForeignKey("NextId")]
        public Scenario? Next { get; set; }

        public GameInfo? Info { get; set; }
        public List<ScenarioPlayers> Players { get; set; } = new List<ScenarioPlayers>();
        public List<SaveGame> SaveGames { get; set; } = new List<SaveGame>();
    }

    public class ScenarioPlayers
    {
        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; } = null!;

        public int ArmyId { get; set; }
        public Army Army { get; set; } = null!;

        public byte Position { get; set; }
    }
}
