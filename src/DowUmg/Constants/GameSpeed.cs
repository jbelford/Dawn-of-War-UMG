using System;

namespace DowUmg.Constants
{
    public enum GameSpeed : int
    {
        VERY_SLOW,
        SLOW,
        NORMAL,
        FAST
    }

    public static class GameSpeedEx
    {
        public static string GetName(this GameSpeed val) =>
            val switch
            {
                GameSpeed.VERY_SLOW => "Very Slow",
                GameSpeed.SLOW => "Slow",
                GameSpeed.NORMAL => "Normal",
                GameSpeed.FAST => "Fast",
                _
                    => throw new ArgumentException(
                        message: "invalid enum value",
                        paramName: nameof(val)
                    )
            };
    }
}
