using System;

namespace DowUmg.Enums
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
        public static string ToString(this GameDifficulty val) => val switch
        {
            GameDifficulty.ANY => "Any",
            GameDifficulty.EASY => "Easy",
            GameDifficulty.STANDARD => "Standard",
            GameDifficulty.HARD => "Hard",
            GameDifficulty.HARDER => "Harder",
            GameDifficulty.INSANE => "Insane",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(val))
        };
    }
}
