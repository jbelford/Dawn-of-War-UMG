using DowUmg.Constants;
using DowUmg.Data.Entities;
using System;
using System.Collections.Generic;

namespace DowUmg.Models
{
    public class GenerationSettings
    {
        public DowMod Mod { get; set; }
        public List<DowMap> Maps { get; set; }
        public List<GameRule> Rules { get; set; }

        #region Team Settings

        public TeamSettings? Teams { get; }

        public class TeamSettings
        {
            public bool EvenDistribution { get; }
            public List<TeamPlayerSettings> Players { get; } = new List<TeamPlayerSettings>();
        }

        public class TeamPlayerSettings
        {
            public int Min { get; }
            public int Max { get; }
            public int Humans { get; }
        }

        #endregion Team Settings

        #region Game Options

        public int[] GameDifficultyTickets { get; } = new int[Enum.GetValues(typeof(GameDifficulty)).Length];
        public int[] GameSpeedTickets { get; } = new int[Enum.GetValues(typeof(GameSpeed)).Length];
        public int[] ResourceRateTickets { get; } = new int[Enum.GetValues(typeof(GameResourceRate)).Length];
        public int[] StartResourceTickets { get; } = new int[Enum.GetValues(typeof(GameStartResource)).Length];

        #endregion Game Options
    }
}
