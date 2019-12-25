using DowUmg.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DowUmg.FileFormats
{
    /**
     * Locales between 15000000 – 20000000  are reserved for mods. (5 million possible)
     * If there are conflicts then the game will crash. But we can not take advantage of this.
     * UCS file always has a 2 byte header: 0xFEFF
     */

    public class LocaleStore
    {
        private readonly Locales[] directLocales;
        private readonly Regex reg = new Regex(@"\$(\d+)");

        public LocaleStore()
        {
            this.directLocales = new Locales[0];
        }

        public LocaleStore(Locales[] locales)
        {
            this.directLocales = locales;
        }

        public List<LocaleStore> Dependencies { get; } = new List<LocaleStore>();

        public string Replace(string input)
        {
            return this.reg.Replace(input, (Match match) => GetValue(int.Parse(match.Groups[1].Value)) ?? "<NOT FOUND>");
        }

        private string? GetValue(int num)
        {
            foreach (var locales in this.directLocales)
            {
                if (locales.ContainsKey(num))
                {
                    return locales[num];
                }
            }

            foreach (var dependency in Dependencies)
            {
                string? value = dependency.GetValue(num);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }

    public class Locales
    {
        private readonly Dictionary<int, string> mappings;

        public Locales(Dictionary<int, string> mappings)
        {
            this.mappings = mappings;
        }

        public string this[int num]
        {
            get => this.mappings[num];
        }

        public bool ContainsKey(int num)
        {
            return this.mappings.ContainsKey(num);
        }
    }

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
            var mappings = new Dictionary<int, string>();

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
                            mappings.Add(key, match.Groups.Count > 2 ? match.Groups[2].Value : "");
                        }
                    }
                }
            }

            return new Locales(mappings);
        }
    }
}
