using System.IO;
using System.Text.RegularExpressions;
using DowUmg.Interfaces;

namespace DowUmg.FileFormats
{
    /**
     * Locales between 15000000 – 20000000  are reserved for mods. (5 million possible)
     * If there are conflicts then the game will crash. But we can not take advantage of this.
     * UCS file always has a 2 byte header: 0xFEFF
     */

    internal class LocaleLoader : IFileLoader<Locales>
    {
        private readonly Regex reg = new Regex(@"^(\d+)(?:[\s\t]+(.*))?$");

        public Locales Load(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Load(stream);
        }

        public Locales Load(Stream stream)
        {
            var locales = new Locales();

            using (var r = new StreamReader(stream))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();
                    if (line.Trim().Length > 0)
                    {
                        Match match = this.reg.Match(line);
                        if (match.Success)
                        {
                            int key = int.Parse(match.Groups[1].Value);
                            locales.Add(key, match.Groups.Count > 2 ? match.Groups[2].Value : "");
                        }
                    }
                }
            }

            return locales;
        }
    }
}
