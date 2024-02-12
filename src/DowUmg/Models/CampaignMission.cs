using System.Collections.Generic;
using MessagePack;

namespace DowUmg.Models
{
    [MessagePackObject]
    public class CampaignMission(CampaignMap map)
    {
        [Key(0)]
        public string Name { get; set; } = "";

        [Key(1)]
        public string Description { get; set; } = "";

        [Key(2)]
        public CampaignMap Map { get; set; } = map;

        [Key(3)]
        public IList<CampaignRule> Rules { get; set; } = new List<CampaignRule>();

        [Key(4)]
        public IList<CampaignMissionTeam> MissionTeams { get; set; } =
            new List<CampaignMissionTeam>();
    }

    [MessagePackObject]
    public class CampaignMap
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Details { get; set; }

        [Key(2)]
        public int Players { get; set; }

        [Key(3)]
        public int Size { get; set; }

        [Key(4)]
        public string Image { get; set; } = null!;

        [IgnoreMember]
        public string? ImagePath { get; set; }
    }
}
