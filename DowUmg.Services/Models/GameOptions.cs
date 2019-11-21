using DowUmg.Services.Enums;

namespace DowUmg.Services.Models
{
    public class GameOptions
    {
        public GameDifficulty Difficulty { get; } = GameDifficulty.STANDARD;
        public GameResourceRate ResourceRate { get; } = GameResourceRate.STANDARD;
        public GameSpeed Speed { get; } = GameSpeed.NORMAL;
        public GameStartResource StartingResources { get; } = GameStartResource.STANDARD;
        public bool EnableCheats { get; } = false;
        public bool LockTeams { get; } = false;
        public bool RandomPosition { get; } = false;
        public bool RandomTeams { get; } = false;
        public bool Sharing { get; } = true;
    }
}
