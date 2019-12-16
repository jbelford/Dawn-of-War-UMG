using DowUmg.FileFormats;
using System;
using System.Collections.Generic;

namespace DowUmg.Interfaces
{
    internal interface IModuleDataExtractor : IDisposable
    {
        public IEnumerable<Locales> GetLocales();

        public IEnumerable<MapFile> GetMaps();

        public string? GetMapImage(string fileName);

        public IEnumerable<GameRuleFile> GetGameRules();

        public IEnumerable<RaceFile> GetRaces();
    }
}
