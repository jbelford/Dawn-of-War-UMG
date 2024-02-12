using System.Collections.Generic;
using MessagePack;

namespace DowUmg.Models
{
    [MessagePackObject]
    public class CampaignMissionTeam
    {
        [Key(0)]
        public IList<CampaignMissionPlayer> MissionMembers { get; set; } =
            new List<CampaignMissionPlayer>();
    }
}
