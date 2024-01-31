namespace DowUmg.Constants
{
    public static class DowConstants
    {
        public static string DXP2Folder = "dxp2";
        public static string W40kFolder = "w40k";

        public static bool IsVanilla(string folderName) =>
            DXP2Folder == folderName || W40kFolder == folderName;
    }
}
