namespace DowUmg.Models
{
    public class SaveGame
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Campaign Campaign { get; set; } = null!;
        public Scenario Scenario { get; set; } = null!;
    }
}
