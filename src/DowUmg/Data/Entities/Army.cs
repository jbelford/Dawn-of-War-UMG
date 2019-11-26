﻿using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class Army
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;
        public string Race { get; set; } = null!;
        public Alliance? Alliance { get; set; }
        public string? Image { get; set; }

        public List<ScenarioPlayers> Scenarios { get; set; } = new List<ScenarioPlayers>();
    }
}
