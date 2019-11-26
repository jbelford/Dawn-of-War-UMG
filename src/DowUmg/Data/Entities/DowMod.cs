using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class DowMod
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public List<DowMap> Maps { get; } = new List<DowMap>();
        public List<GameRule> Rules { get; } = new List<GameRule>();
    }
}
