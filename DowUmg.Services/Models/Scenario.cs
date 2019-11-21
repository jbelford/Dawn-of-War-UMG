using System.Collections.Generic;

namespace DowUmg.Services.Models
{
    public class Scenario : InfoObject
    {
        public GameInfo Info { get; }
        public DowMap Map { get; }
        public List<Army> Players { get; } = new List<Army>();
    }
}
