using System;

namespace DowUmg.Constants
{
    public enum GameDifficulty : int
    {
        EASY,
        STANDARD,
        HARD,
        HARDER,
        INSANE
    }

    public static class GameDifficultyEx
    {
        public static string ToString(this GameDifficulty val) =>
            val switch
            {
                GameDifficulty.EASY => "Easy",
                GameDifficulty.STANDARD => "Standard",
                GameDifficulty.HARD => "Hard",
                GameDifficulty.HARDER => "Harder",
                GameDifficulty.INSANE => "Insane",
                _
                    => throw new ArgumentException(
                        message: "invalid enum value",
                        paramName: nameof(val)
                    )
            };
    }
}
