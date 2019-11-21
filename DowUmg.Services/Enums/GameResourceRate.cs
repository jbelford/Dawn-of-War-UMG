namespace DowUmg.Services.Enums
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
        public static string ToString(this GameResourceRate val)
        {
            switch (val)
            {
                default:
                case GameResourceRate.ANY:
                    return "Any";

                case GameResourceRate.LOW:
                    return "Low";

                case GameResourceRate.STANDARD:
                    return "Standard";

                case GameResourceRate.HIGH:
                    return "High";
            }
        }
    }
}
