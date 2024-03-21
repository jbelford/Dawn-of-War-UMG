using System;
using System.IO;
using DowUmg.Data.Entities;
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
        private readonly DowModLoader modLoader;
        private readonly IModDataService modDataService;

        public CampaignService()
        {
            logger = this.Log();
            modLoader = Locator.Current.GetService<DowModLoader>()!;
            modDataService = Locator.Current.GetService<IModDataService>()!;
        }

        public CampaignMap GetDefaultCampaignMap()
        {
            DowMap map = modDataService.GetDefaultMap();
            return ConvertMapEntity(map);
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

        private CampaignMap ConvertMapEntity(DowMap map) =>
            new()
            {
                Name = map.Name,
                Details = map.Details,
                Players = map.Players,
                Size = map.Size,
                Image = map.Image,
                ImagePath = modLoader.GetMapImagePath(map)
            };
    }
}
