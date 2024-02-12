using DowUmg.Data.Entities;
using MessagePack;

namespace DowUmg.Models
{
    [MessagePackObject]
    public class CampaignMissionPlayer
    {
        [Key(0)]
        public bool? Player { get; set; }

        [Key(1)]
        public CampaignRace? Race { set; get; }

        [Key(2)]
        public int Position { get; set; }
    }

    [MessagePackObject]
    public class CampaignRace
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Description { get; set; }
    }
}
