namespace DowUmg.Data.Entities
{
    public class DowRace
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DowMod Mod { get; set; } = null!;
    }
}
