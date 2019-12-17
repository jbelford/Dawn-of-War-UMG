using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class DowMod
    {
        public bool IsVanilla { get; set; }
        public string ModFolder { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public List<DowMap> Maps { get; } = new List<DowMap>();
        public List<GameRule> Rules { get; } = new List<GameRule>();
        public List<DowRace> Races { get; } = new List<DowRace>();
    }
}
