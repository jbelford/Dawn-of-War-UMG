using DowUmg.Data.Entities;
using System.Collections.Generic;

namespace DowUmg.Services
{
    public class LoadMemo
    {
        private Dictionary<string, (DowMod? vanilla, DowMod? mod)> dict = new Dictionary<string, (DowMod? vanilla, DowMod? mod)>();

        public DowMod? Get(string modFolder, bool isVanilla)
        {
            if (this.dict.ContainsKey(modFolder))
            {
                var (vanilla, mod) = this.dict[modFolder];
                return isVanilla ? vanilla : mod;
            }

            return null;
        }

        public void Put(DowMod newMod)
        {
            (DowMod? vanilla, DowMod? mod) value = (null, null);
            if (this.dict.ContainsKey(newMod.ModFolder))
            {
                value = this.dict[newMod.ModFolder];
            }
            if (newMod.IsVanilla)
            {
                this.dict[newMod.ModFolder] = (newMod, value.mod);
            }
            else
            {
                this.dict[newMod.ModFolder] = (value.vanilla, newMod);
            }
        }
    }
}
