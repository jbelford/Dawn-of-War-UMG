namespace DowUmg.Services.Enums
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
        public static string ToString(this GameSpeed val)
        {
            switch (val)
            {
                default:
                case GameSpeed.ANY:
                    return "Any";

                case GameSpeed.VERY_SLOW:
                    return "Very Slow";

                case GameSpeed.SLOW:
                    return "Slow";

                case GameSpeed.NORMAL:
                    return "Normal";

                case GameSpeed.FAST:
                    return "Fast";
            }
        }
    }
}
