using DowUmg.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DowUmg.FileFormats
{
    public class UcsLoader : IFileLoader<Dictionary<int, string>>
    {
        public Dictionary<int, string> Load(string filePath)
        {
            var locales = new Dictionary<int, string>();

            using (var r = new StreamReader(filePath))
            {
                while (!r.EndOfStream)
                {
                    string[] line = Regex.Split(r.ReadLine(), @"\s+");
                    int id = int.Parse(line[0]);
                    locales[id] = line.Length > 1 ? line[1] : "";
                }
            }

            return locales;
        }
    }
}
