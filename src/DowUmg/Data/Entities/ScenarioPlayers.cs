namespace DowUmg.Data.Entities
{
    public class ScenarioPlayers
    {
        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; } = null!;

        public int ArmyId { get; set; }
        public Army Army { get; set; } = null!;

        public byte Position { get; set; }
    }
}
