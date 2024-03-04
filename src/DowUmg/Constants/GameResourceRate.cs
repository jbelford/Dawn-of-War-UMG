using System;

namespace DowUmg.Constants
{
    public enum GameResourceRate : int
    {
        LOW,
        STANDARD,
        HIGH
    }

    public static class GameResourceRateEx
    {
        public static string GetName(this GameResourceRate val) =>
            val switch
            {
                GameResourceRate.LOW => "Low",
                GameResourceRate.STANDARD => "Standard",
                GameResourceRate.HIGH => "High",
                _
                    => throw new ArgumentException(
                        message: "invalid enum value",
                        paramName: nameof(val)
                    )
            };
    }
}
