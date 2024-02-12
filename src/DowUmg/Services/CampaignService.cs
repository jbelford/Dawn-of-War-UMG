using System;
using System.IO;
using DowUmg.Data;
using DowUmg.Data.Entities;
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
        private readonly DowModLoader modLoader;

        public CampaignService()
        {
            logger = this.Log();
            modLoader = Locator.Current.GetService<DowModLoader>()!;
        }

        public CampaignMap GetDefaultCampaignMap()
        {
            using var dataStore = new ModsDataStore();
            DowMap map = dataStore.GetDowMap();
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
