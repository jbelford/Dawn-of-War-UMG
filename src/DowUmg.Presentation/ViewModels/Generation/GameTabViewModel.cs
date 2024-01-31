using System;
using System.Linq;
using DowUmg.Constants;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class GameTabViewModel : ReactiveObject
    {
        public GameTabViewModel()
        {
            DiffOption = new ProportionalOptionsViewModel<GameDifficulty>(
                "Difficulty",
                GameDifficultyEx.ToString,
                Enum.GetValues(typeof(GameDifficulty)).Cast<GameDifficulty>().ToArray()
            );
            SpeedOption = new ProportionalOptionsViewModel<GameSpeed>(
                "Game Speed",
                GameSpeedEx.ToString,
                Enum.GetValues(typeof(GameSpeed)).Cast<GameSpeed>().ToArray()
            );
            RateOption = new ProportionalOptionsViewModel<GameResourceRate>(
                "Resource Rate",
                GameResourceRateEx.ToString,
                Enum.GetValues(typeof(GameResourceRate)).Cast<GameResourceRate>().ToArray()
            );
            StartingOption = new ProportionalOptionsViewModel<GameStartResource>(
                "Starting Resources",
                GameStartResourceEx.ToString,
                Enum.GetValues(typeof(GameStartResource)).Cast<GameStartResource>().ToArray()
            );
        }

        public ProportionalOptionsViewModel<GameDifficulty> DiffOption { get; set; }
        public ProportionalOptionsViewModel<GameSpeed> SpeedOption { get; set; }
        public ProportionalOptionsViewModel<GameResourceRate> RateOption { get; set; }
        public ProportionalOptionsViewModel<GameStartResource> StartingOption { get; set; }
    }
}
