using System;
using System.Collections.Generic;
using DowUmg.FileFormats;

namespace DowUmg.Interfaces
{
    internal interface IModuleDataExtractor : IDisposable
    {
        IEnumerable<MapFile> GetMaps();

        string? GetMapImage(string fileName);

        IEnumerable<GameRuleFile> GetGameRules();

        IEnumerable<RaceFile> GetRaces();
    }
}
