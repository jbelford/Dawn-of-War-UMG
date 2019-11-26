namespace DowUmg.Data.Entities
{
    public class DowMap
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DowMod Mod { get; set; } = null!;
        public byte Players { get; set; }
        public byte Size { get; set; }
        public string Image { get; set; } = null!;
    }
}
