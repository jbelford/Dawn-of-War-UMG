using System;
using System.Collections.Generic;
using DowUmg.FileFormats;

namespace DowUmg.Services
{
    internal interface IModuleDataExtractor : IDisposable
    {
        IEnumerable<MapFile> GetMaps();

        string? GetMapImage(string fileName);

        IEnumerable<GameRuleFile> GetGameRules();

        IEnumerable<RaceFile> GetRaces();
    }
}
