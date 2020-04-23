namespace DowUmg.Constants
{
    public static class DowConstants
    {
        public static string DXP2Folder = "DXP2";
        public static string W40kFolder = "W40k";

        public static bool IsVanilla(string str) =>
            DXP2Folder.Equals(str, System.StringComparison.OrdinalIgnoreCase)
                || W40kFolder.Equals(str, System.StringComparison.OrdinalIgnoreCase);
    }
}
