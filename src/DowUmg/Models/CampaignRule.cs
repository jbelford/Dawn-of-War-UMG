using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace DowUmg.Models
{
    [MessagePackObject]
    public class CampaignRule
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public string Details { get; set; }

        [Key(2)]
        public bool IsWinCondition { get; set; }
    }
}
