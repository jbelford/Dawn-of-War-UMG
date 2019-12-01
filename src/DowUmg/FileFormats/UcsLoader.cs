using DowUmg.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DowUmg.FileFormats
{
    public class Locales
    {
        protected readonly Dictionary<string, string> mappings;

        public Locales(Dictionary<string, string> mappings)
        {
            this.mappings = mappings;
        }

        public string Replace(string input)
        {
            Match match = Regex.Match(input, @"\$(\d+)");
            return match.Success ? mappings[match.Groups[1].Value] : input;
        }

        public Locales Concat(Locales value)
        {
            foreach (var kvp in value.mappings)
            {
                this.mappings[kvp.Key] = kvp.Value;
            }
            return this;
        }
    }

    public class UcsLoader : IFileLoader<Locales>
    {
        private readonly Regex reg = new Regex(@"^(\d+)(?:[\s\t]+(.*))?$");

        public Locales Load(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Load(stream);
        }

        public Locales Load(Stream stream)
        {
            var mappings = new Dictionary<string, string>();

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
                            mappings.Add(match.Groups[1].Value, match.Groups.Count > 2 ? match.Groups[2].Value : "");
                        }
                    }
                }
            }

            return new Locales(mappings);
        }
    }
}
