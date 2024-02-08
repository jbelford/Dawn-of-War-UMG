using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public class Campaign
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public IList<CampaignMission> Missions { get; set; } = new List<CampaignMission>();
    }

    public class CampaignMission(DowMap map)
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public DowMap Map { get; set; } = map;

        public IList<GameRule> Rules { get; set; } = new List<GameRule>();

        public IList<CampaignMissionTeam> MissionTeams { get; set; } =
            new List<CampaignMissionTeam>();
    }

    public class CampaignMissionTeam
    {
        public IList<CampaignMissionPlayer> MissionMembers { get; set; } =
            new List<CampaignMissionPlayer>();
    }

    public class CampaignMissionPlayer
    {
        public bool? Player { get; set; }

        public DowRace? Race { set; get; }

        public int Position { get; set; }
    }
}
