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
            using var stream = File.OpenRead(filePath);
            return Load(stream);
        }

        public Dictionary<int, string> Load(Stream stream)
        {
            var locales = new Dictionary<int, string>();

            using (var r = new StreamReader(stream))
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
