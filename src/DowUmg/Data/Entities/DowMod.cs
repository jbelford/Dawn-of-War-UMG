using System.Collections.Generic;

namespace DowUmg.Data.Entities
{
    public class DowMod
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string ModFile { get; set; } = null!;
        public bool IsVanilla { get; set; }
        public bool Playable { get; set; }
        public virtual DowModData Data { get; set; }
        public virtual ICollection<DowMod> Dependencies { get; set; }
        public virtual ICollection<DowMod> Dependents { get; set; }
    }

    public class DowModData
    {
        public int Id { get; set; }
        public string ModFolder { get; set; } = null!;
        public virtual ICollection<DowMod> Mods { get; set; }
        public virtual ICollection<DowMap> Maps { get; set; }
        public virtual ICollection<GameRule> Rules { get; set; }
        public virtual ICollection<DowRace> Races { get; set; }
    }
}
