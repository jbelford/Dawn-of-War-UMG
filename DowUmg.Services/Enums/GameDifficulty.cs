namespace DowUmg.Services.Enums
{
    public enum GameDifficulty
    {
        ANY,
        EASY,
        STANDARD,
        HARD,
        HARDER,
        INSANE
    }

    public static class GameDifficultyEx
    {
        public static string ToString(this GameDifficulty val)
        {
            switch (val)
            {
                default:
                case GameDifficulty.ANY:
                    return "Any";

                case GameDifficulty.EASY:
                    return "Easy";

                case GameDifficulty.STANDARD:
                    return "Standard";

                case GameDifficulty.HARD:
                    return "Hard";

                case GameDifficulty.HARDER:
                    return "Harder";

                case GameDifficulty.INSANE:
                    return "Insane";
            }
        }
    }
}
