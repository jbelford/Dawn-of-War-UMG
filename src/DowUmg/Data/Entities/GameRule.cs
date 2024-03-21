namespace DowUmg.Data.Entities
{
    public class GameRule
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public virtual DowModData Mod { get; set; } = null!;
        public bool IsWinCondition { get; set; }
    }
}
