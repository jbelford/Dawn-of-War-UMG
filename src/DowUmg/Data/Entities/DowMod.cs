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
        public virtual ICollection<DowModDependency> Dependencies { get; set; }
        public virtual ICollection<DowModDependency> Dependents { get; set; }
    }

    public class DowModDependency
    {
        public int MainModId { get; set; }
        public virtual DowMod MainMod { get; set; } = null!;
        public int DepModId { get; set; }
        public virtual DowMod DepMod { get; set; } = null!;
    }
}
