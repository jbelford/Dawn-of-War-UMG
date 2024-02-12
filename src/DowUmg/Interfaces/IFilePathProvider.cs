﻿namespace DowUmg.Interfaces
{
    public interface IFilePathProvider
    {
        string ModCacheLocation { get; }

        string SettingsLocation { get; }

        string DataLocation { get; }

        string SoulstormLocation { get; }

        string CampaignsLocation { get; }
    }
}
