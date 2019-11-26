﻿using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class Alliance
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;
        public List<Army> Armies { get; } = new List<Army>();
        public string? Image { get; set; }
    }
}
