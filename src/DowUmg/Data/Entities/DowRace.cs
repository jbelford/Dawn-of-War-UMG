namespace DowUmg.Data.Entities
{
    public class DowRace
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual DowMod Mod { get; set; } = null!;
    }
}
