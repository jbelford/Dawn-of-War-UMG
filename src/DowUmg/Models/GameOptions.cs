using DowUmg.Constants;

namespace DowUmg.Models
{
    public class GameOptions
    {
        public GameDifficulty? Difficulty { get; set; }
        public GameResourceRate? ResourceRate { get; set; }
        public GameSpeed? Speed { get; set; }
        public GameStartResource? StartingResources { get; set; }
        public bool? RandomPosition { get; set; }
        public bool? RandomTeams { get; set; }
        public bool? Sharing { get; set; }
    }
}
