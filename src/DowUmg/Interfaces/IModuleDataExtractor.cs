using DowUmg.FileFormats;
using System;
using System.Collections.Generic;

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
