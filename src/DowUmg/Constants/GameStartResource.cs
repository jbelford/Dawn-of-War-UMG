using System;

namespace DowUmg.Constants
{
    public enum GameStartResource
    {
        ANY,
        STANDARD,
        QUICK
    }

    public static class GameStartResourceEx
    {
        public static string ToString(this GameStartResource val) => val switch
        {
            GameStartResource.ANY => "Any",
            GameStartResource.STANDARD => "Standard",
            GameStartResource.QUICK => "Quick",
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(val))
        };
    }
}
