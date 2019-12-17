using System;

namespace DowUmg.Constants
{
    public enum GameResourceRate
    {
        ANY,
        LOW,
        STANDARD,
        HIGH
    }

    public static class GameResourceRateEx
    {
        public static string ToString(this GameResourceRate val) => val switch
        {
            GameResourceRate.ANY => "Any",
            GameResourceRate.LOW => "Low",
            GameResourceRate.STANDARD => "Standard",
            GameResourceRate.HIGH => "High",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(val))
        };
    }
}
