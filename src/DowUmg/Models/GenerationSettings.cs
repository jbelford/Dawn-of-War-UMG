using System;
using System.Collections.Generic;
using DowUmg.Constants;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public class GenerationSettings
    {
        public DowMod Mod { get; set; }
        public List<DowMap> Maps { get; set; }
        public List<GameRule> Rules { get; set; }
        public List<DowRace> Races { get; set; }

        public List<string> Players { get; set; }

        public int MinComputer { get; set; }
        public int MaxComputer { get; set; }

        public int MinTeams { get; set; }
        public int MaxTeams { get; set; }

        public bool RandomPositions { get; set; }

        public bool OneRaceTeams { get; set; }

        public int[] GameDifficultyTickets { get; } =
            new int[Enum.GetValues(typeof(GameDifficulty)).Length];
        public int[] GameSpeedTickets { get; } = new int[Enum.GetValues(typeof(GameSpeed)).Length];
        public int[] ResourceRateTickets { get; } =
            new int[Enum.GetValues(typeof(GameResourceRate)).Length];
        public int[] StartResourceTickets { get; } =
            new int[Enum.GetValues(typeof(GameStartResource)).Length];
    }
}
