using System.Collections.Generic;

namespace DowUmg.Constants
{
    public static class DowConstants
    {
        private static readonly ISet<string> VanillaFolder = new HashSet<string>() { "w40k", "wxp", "dxp2", "dxp3", "dowde" };

        public static bool IsVanilla(string folderName) => VanillaFolder.Contains(folderName);
    }
}
