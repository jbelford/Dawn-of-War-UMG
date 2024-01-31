using System;
using System.Linq;
using DowUmg.Constants;
using DowUmg.Data.Entities;
using DowUmg.Models;

namespace DowUmg.Services
{
    public class GenerationService
    {
        public GenerationService() { }

        public Matchup GenerateMatchup(GenerationSettings settings)
        {
            var random = new Random();

            DowMap map = settings.Maps[random.Next(settings.Maps.Count)];

            var info = new GameInfo()
            {
                Options = new GameOptions()
                {
                    Difficulty = (GameDifficulty)RandomOption(
                        settings.GameDifficultyTickets,
                        random
                    ),
                    Speed = (GameSpeed)RandomOption(settings.GameSpeedTickets, random),
                    ResourceRate = (GameResourceRate)RandomOption(
                        settings.ResourceRateTickets,
                        random
                    ),
                    StartingResources = (GameStartResource)RandomOption(
                        settings.StartResourceTickets,
                        random
                    )
                }
            };

            info.Rules.Add(settings.Rules[random.Next(settings.Rules.Count)]);

            var matchup = new Matchup(map, info);

            if (settings.Teams != null)
            {
                // TODO generate the team compositions
            }

            return matchup;
        }

        private int RandomOption(int[] enumeration, Random random)
        {
            int idx = random.Next(enumeration.Sum() + 1);

            int i = 0;
            while (idx > 0)
            {
                idx -= enumeration[i++];
            }
            return --i;
        }
    }
}
