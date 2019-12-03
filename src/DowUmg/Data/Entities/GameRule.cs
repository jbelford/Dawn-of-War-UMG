namespace DowUmg.Data.Entities
{
    public class GameRule
    {
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DowMod Mod { get; set; } = null!;
        public bool IsWinCondition { get; set; }
    }
}
