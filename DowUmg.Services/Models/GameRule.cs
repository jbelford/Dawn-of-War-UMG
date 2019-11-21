namespace DowUmg.Services.Models
{
    public class GameRule : InfoObject
    {
        public bool IsWinCondition { get; }
        public bool AlwaysOn { get; }
        public bool Exclusive { get; }
    }
}
