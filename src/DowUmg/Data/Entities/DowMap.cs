namespace DowUmg.Data.Entities
{
    public class DowMap
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public virtual DowModData Mod { get; set; } = null!;
        public int Players { get; set; }
        public int Size { get; set; }
        public string Image { get; set; } = null!;
    }
}
