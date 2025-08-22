using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DowUmg.FileFormats
{
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

        public List<LocaleStore> Dependencies { get; set; } = new List<LocaleStore>();

        public string Replace(string input)
        {
            return this.reg.Replace(
                input,
                (Match match) =>
                    GetValue(int.Parse(match.Groups[1].Value)) ?? $"<NOT FOUND - {input}>"
            );
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

    public class Locales : Dictionary<int, string> { }
}
