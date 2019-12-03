namespace DowUmg.Data.Entities
{
    public class DowMap
    {
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DowMod Mod { get; set; } = null!;
        public int Players { get; set; }
        public int Size { get; set; }
        public string Image { get; set; } = null!;
    }
}
