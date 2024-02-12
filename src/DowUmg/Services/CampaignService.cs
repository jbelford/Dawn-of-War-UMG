using System;
using System.Collections.Generic;
using System.IO;
using DowUmg.Interfaces;
using DowUmg.Models;
using MessagePack;
using Splat;

namespace DowUmg.Services
{
    public class CampaignIOException(string message, Exception inner)
        : Exception(message, inner) { }

    public class CampaignService : ICampaignService, IEnableLogger
    {
        private ILogger logger;

        public CampaignService()
        {
            logger = this.Log();
        }

        public IList<Campaign> GetCampaignList()
        {
            throw new NotImplementedException();
        }

        public void SaveCampaign(string filePath, Campaign campaign)
        {
            try
            {
                byte[] data = MessagePackSerializer.Serialize(campaign);
                File.WriteAllBytes(filePath, data);
            }
            catch (MessagePackSerializationException ex)
            {
                logger.Write(ex, "Error serializing campaign!", LogLevel.Error);
                throw new CampaignIOException("Failed to save campaign!", ex);
            }
        }

        public Campaign ReadCampaign(string filePath)
        {
            try
            {
                byte[] data = File.ReadAllBytes(filePath);
                return MessagePackSerializer.Deserialize<Campaign>(data);
            }
            catch (MessagePackSerializationException ex)
            {
                logger.Write(ex, "Error serializing campaign!", LogLevel.Error);
                throw new CampaignIOException("Failed to read campaign!", ex);
            }
        }
    }
}
