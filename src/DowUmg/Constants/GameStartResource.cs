﻿using System;

namespace DowUmg.Constants
{
    public enum GameStartResource : int
    {
        STANDARD,
        QUICK
    }

    public static class GameStartResourceEx
    {
        public static string GetName(this GameStartResource val) =>
            val switch
            {
                GameStartResource.STANDARD => "Standard",
                GameStartResource.QUICK => "Quick",
                _
                    => throw new ArgumentException(
                        message: "invalid enum value",
                        paramName: nameof(val)
                    )
            };
    }
}
