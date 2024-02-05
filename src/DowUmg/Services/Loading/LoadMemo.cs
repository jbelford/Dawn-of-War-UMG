using System.Collections.Generic;
using DowUmg.Data.Entities;
using DowUmg.FileFormats;

namespace DowUmg.Services
{
    public class LoadMemo
    {
        private readonly Dictionary<string, (DowMod? vanilla, DowMod? mod)> modMemo = [];
        private readonly Dictionary<string, (DowModData? vanilla, DowModData? mod)> dataMemo = [];

        public DowMod? GetMod(DowModuleFile moduleFile)
        {
            if (modMemo.TryGetValue(moduleFile.FileName, out (DowMod? vanilla, DowMod? mod) value))
            {
                var (vanilla, mod) = value;
                return moduleFile.IsVanilla ? vanilla : mod;
            }

            return null;
        }

        public void PutMod(DowMod newMod)
        {
            modMemo.TryGetValue(newMod.ModFile, out (DowMod? vanilla, DowMod? mod) value);
            if (newMod.IsVanilla)
            {
                this.modMemo[newMod.ModFile] = (newMod, value.mod);
            }
            else
            {
                this.modMemo[newMod.ModFile] = (value.vanilla, newMod);
            }
        }

        public DowModData? GetData(DowModuleFile moduleFile)
        {
            if (
                dataMemo.TryGetValue(
                    moduleFile.ModFolder,
                    out (DowModData? vanilla, DowModData? mod) value
                )
            )
            {
                var (vanilla, mod) = value;
                return moduleFile.IsVanilla ? vanilla : mod;
            }

            return null;
        }

        public void PutData(DowModData newData, bool isVanilla)
        {
            dataMemo.TryGetValue(
                newData.ModFolder,
                out (DowModData? vanilla, DowModData? mod) value
            );
            if (isVanilla)
            {
                this.dataMemo[newData.ModFolder] = (newData, value.mod);
            }
            else
            {
                this.dataMemo[newData.ModFolder] = (value.vanilla, newData);
            }
        }
    }
}
