using System.Collections.Generic;
using MessagePack;

namespace DowUmg.Models
{
    [MessagePackObject]
    public class Campaign
    {
        [Key(0)]
        public string Name { get; set; } = "";

        [Key(1)]
        public string Description { get; set; } = "";

        [Key(2)]
        public IList<CampaignMission> Missions { get; set; } = new List<CampaignMission>();
    }
}
