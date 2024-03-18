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

            int humans = settings.Players.Count;
            int remainingPlayers = map.Players - humans;

            playerList.AddRange(
                settings.Players.Select(
                    (player, idx) =>
                        new MatchupPlayer(player.Length > 0 ? player : $"Player {idx + 1}", 0)
                        {
                            Position = idx
                        }
                )
            );

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

                    playerList.Add(
                        new MatchupPlayer("Computer", team, new(race.Name, race.FileName))
                    );
                }
            }

            int startIdx = settings.RandomPositions ? 1 : humans;
            var positions = Enumerable.Range(startIdx, map.Players - startIdx).ToList();

            for (int i = 0; i < positions.Count; ++i)
            {
                int randomIdx = random.Next(0, positions.Count);
                (positions[randomIdx], positions[i]) = (positions[i], positions[randomIdx]);
            }

            for (int i = startIdx; i < playerList.Count; ++i)
            {
                playerList[i].Position = positions[i - startIdx];
            }

            playerList.Sort((a, b) => a.Position - b.Position);

            return new Matchup(map, info, playerList);
        }

        private int RandomOption(int[] enumeration, Random random)
        {
            int idx = random.Next(enumeration.Sum() + 1);

            for (int i = 0; i < enumeration.Length; ++i)
            {
                idx -= enumeration[i];
                if (idx < 0)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
