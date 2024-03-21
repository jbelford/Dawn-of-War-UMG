using DowUmg.Models;

namespace DowUmg.Services
{
    public interface ICampaignService
    {
        public CampaignMap GetDefaultCampaignMap();

        public void SaveCampaign(string filePath, Campaign campaign);

        public Campaign ReadCampaign(string filePath);
    }
}
