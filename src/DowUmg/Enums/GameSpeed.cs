using System;

namespace DowUmg.Enums
{
    public enum GameSpeed
    {
        ANY,
        VERY_SLOW,
        SLOW,
        NORMAL,
        FAST
    }

    public static class GameSpeedEx
    {
        public static string ToString(this GameSpeed val) => val switch
        {
            GameSpeed.ANY => "Any",
            GameSpeed.VERY_SLOW => "Very Slow",
            GameSpeed.SLOW => "Slow",
            GameSpeed.NORMAL => "Normal",
            GameSpeed.FAST => "Fast",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(val))
        };
    }
}
