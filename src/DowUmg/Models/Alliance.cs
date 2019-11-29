using System.Collections.Generic;

namespace DowUmg.Models
{
    public class Alliance
    {
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;
        public List<Army> Armies { get; } = new List<Army>();
        public string? Image { get; set; }
    }
}
