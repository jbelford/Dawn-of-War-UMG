using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class DowMod
    {
        public int Id { get; set; }
        public bool IsVanilla { get; set; }
        public string ModFolder { get; set; } = null!;
        public bool Playable { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public virtual ICollection<DowMap> Maps { get; set; }
        public virtual ICollection<GameRule> Rules { get; set; }
        public virtual ICollection<DowRace> Races { get; set; }
        public virtual ICollection<DowMod> Dependencies { get; set; }
        public virtual ICollection<DowMod> Dependents { get; set; }
    }
}
