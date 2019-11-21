namespace DowUmg.Services.Enums
{
    public enum GameStartResource
    {
        ANY,
        STANDARD,
        QUICK
    }

    public static class GameStartResourceEx
    {
        public static string ToString(this GameStartResource val)
        {
            switch (val)
            {
                default:
                case GameStartResource.ANY:
                    return "Any";

                case GameStartResource.STANDARD:
                    return "Standard";

                case GameStartResource.QUICK:
                    return "Quick";
            }
        }
    }
}
