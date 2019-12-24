using DowUmg.Data.Entities;
using DowUmg.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DowUmg.Services
{
    public class GenerationService
    {
        private readonly List<DowMod> mods;

        public GenerationService(IEnumerable<DowMod> mods)
        {
            this.mods = mods.ToList();
        }

        public Matchup GenerateMatchup(int humans, int players)
        {
            List<DowMod> vanillaMods = this.mods.Where(mod => mod.IsVanilla).ToList();

            List<DowMap> maps = vanillaMods.SelectMany(mod => mod.Maps).ToList();
            List<DowRace> races = vanillaMods.SelectMany(mod => mod.Races).ToList();

            var rand = new Random();

            DowMap randomMap = maps[rand.Next(0, maps.Count)];

            var randomRaces = new DowRace[players];
            for (int i = 0; i < players; ++i)
            {
                randomRaces[i] = races[rand.Next(0, races.Count)];
            }

            return null;
        }
    }
}
