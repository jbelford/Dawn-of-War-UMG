using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public List<DowMap> Maps { get; set; } = new List<DowMap>();
        public List<GameRule> Rules { get; set; } = new List<GameRule>();
        public List<DowRace> Races { get; set; } = new List<DowRace>();
        public List<DowModDependency> Dependencies { get; set; } = new List<DowModDependency>();
        public List<DowModDependency> Dependents { get; set; } = new List<DowModDependency>();

        [NotMapped]
        public int MapsCount { get; set; }

        [NotMapped]
        public int RulesCount { get; set; }

        [NotMapped]
        public int RacesCount { get; set; }
    }

    public class DowModDependency
    {
        public int MainModId { get; set; }
        public DowMod MainMod { get; set; } = null!;
        public int DepModId { get; set; }
        public DowMod DepMod { get; set; } = null!;
    }
}
