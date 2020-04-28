namespace DowUmg.Constants
{
    public static class DowConstants
    {
        public static string DXP2Folder = "DXP2";
        public static string W40kFolder = "W40k";

        public static bool IsVanilla(string folderName) =>
            DXP2Folder.Equals(folderName, System.StringComparison.OrdinalIgnoreCase)
                || W40kFolder.Equals(folderName, System.StringComparison.OrdinalIgnoreCase);
    }
}
