using System;
using System.Collections.Generic;
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

            var options = new GameOptions()
            {
                Difficulty = (GameDifficulty)RandomOption(settings.GameDifficultyTickets, random),
                Speed = (GameSpeed)RandomOption(settings.GameSpeedTickets, random),
                ResourceRate = (GameResourceRate)RandomOption(settings.ResourceRateTickets, random),
                StartingResources = (GameStartResource)RandomOption(
                    settings.StartResourceTickets,
                    random
                )
            };
            var info = new GameInfo(options);

            info.Rules.Add(settings.Rules[random.Next(settings.Rules.Count)]);

            var playerList = new List<MatchupPlayer>();

            int maxPlayers = 8;
            int humans = settings.Players.Count;
            int remainingPlayers = maxPlayers - humans;

            foreach (var player in settings.Players)
            {
                playerList.Add(new MatchupPlayer(player, 0));
            }

            if (remainingPlayers > 0)
            {
                int maxComputers = Math.Min(remainingPlayers, settings.MaxComputer);
                int minComputers = Math.Min(remainingPlayers, settings.MinComputer);
                int computers = random.Next(minComputers, maxComputers + 1);

                int teams = random.Next(settings.MinTeams, settings.MaxTeams + 1);

                Dictionary<int, DowRace> teamRaces = [];

                for (int i = 0; i < computers; ++i)
                {
                    int team = random.Next(teams) + 1;
                    DowRace? race = null;
                    if (settings.OneRaceTeams)
                    {
                        teamRaces.TryGetValue(team, out race);
                    }

                    if (race == null)
                    {
                        race = settings.Races[random.Next(settings.Races.Count)];
                        if (settings.OneRaceTeams)
                        {
                            teamRaces.Add(team, race);
                        }
                    }

                    playerList.Add(new MatchupPlayer($"Computer {i + 1}", team, race.Name));
                }
            }

            if (settings.RandomPositions)
            {
                for (int i = 1; i < playerList.Count; ++i)
                {
                    int randomIdx = random.Next(1, playerList.Count);
                    (playerList[randomIdx], playerList[i]) = (playerList[i], playerList[randomIdx]);
                }
            }

            return new Matchup(map, info, playerList);
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
