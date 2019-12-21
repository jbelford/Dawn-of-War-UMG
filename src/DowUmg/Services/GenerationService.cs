using DowUmg.Data.Entities;
using DowUmg.Models;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DowUmg.Services
{
    public class GenerationService
    {
        private readonly DowModService modService;

        public GenerationService(DowModService? modService = null)
        {
            this.modService = modService ?? Locator.Current.GetService<DowModService>();
        }

        public Matchup GenerateMatchup(int humans, int players)
        {
            List<DowMod> mods = this.modService.GetVanillaMods();

            List<DowMap> maps = mods.SelectMany(mod => mod.Maps).ToList();
            List<DowRace> races = mods.SelectMany(mod => mod.Races).ToList();

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
