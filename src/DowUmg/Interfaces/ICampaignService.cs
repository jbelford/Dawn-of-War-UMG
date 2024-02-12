using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowUmg.Models;

namespace DowUmg.Interfaces
{
    public interface ICampaignService
    {
        public void SaveCampaign(string filePath, Campaign campaign);

        public Campaign ReadCampaign(string filePath);
    }
}
