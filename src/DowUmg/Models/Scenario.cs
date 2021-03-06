﻿using DowUmg.Data.Entities;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class Scenario
    {
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;

        public DowMap Map { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;

        public bool InheritedWinConditions { get; set; } = true;
        public bool InheritedGameRules { get; set; } = true;
        public bool InheritedCustomRules { get; set; } = true;
        public bool InheritedGameOptions { get; set; } = true;

        public string? Image { get; set; }

        public Scenario? Previous { get; set; }
        public Scenario? Next { get; set; }

        public GameInfo? Info { get; set; }
        public List<ScenarioPlayers> Players { get; set; } = new List<ScenarioPlayers>();
        public List<SaveGame> SaveGames { get; set; } = new List<SaveGame>();
    }

    public class ScenarioPlayers
    {
        public byte Position { get; set; }
        public Army Army { get; set; } = null!;
    }
}
