namespace DowUmg.Models
{
    public class SaveGame
    {
        public string Name { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;
        public Scenario Scenario { get; set; } = null!;
    }
}
