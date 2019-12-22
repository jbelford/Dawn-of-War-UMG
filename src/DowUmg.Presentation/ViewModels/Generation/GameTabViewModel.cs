using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class GameTabViewModel : ReactiveObject
    {
        public GameTabViewModel()
        {
            DiffOption = new GameOptionViewModel("Difficulty", "Easy", "Standard", "Hard", "Harder", "Insane");
            SpeedOption = new GameOptionViewModel("Game Speed", "Very Slow", "Slow", "Normal", "Fast");
            RateOption = new GameOptionViewModel("Resource Rate", "Low", "Standard", "High");
            StartingOption = new GameOptionViewModel("Starting Resources", "Standard", "Quick-Start");
        }

        public GameOptionViewModel DiffOption { get; set; }
        public GameOptionViewModel SpeedOption { get; set; }
        public GameOptionViewModel RateOption { get; set; }
        public GameOptionViewModel StartingOption { get; set; }
    }
}
