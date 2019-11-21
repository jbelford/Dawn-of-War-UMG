using System.Collections.Generic;

namespace DowUmg.Services.Models
{
    public class DowMod
    {
        public string Name { get; }
        public List<DowMap> Maps { get; } = new List<DowMap>();
        public List<GameRule> Rules { get; } = new List<GameRule>();
    }
}
