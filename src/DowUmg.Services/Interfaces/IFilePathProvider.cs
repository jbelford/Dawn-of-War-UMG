namespace DowUmg.Services.Interfaces
{
    public interface IFilePathProvider
    {
        string AppDataLocation { get; }

        string SettingsLocation { get; }

        string DataLocation { get; }

        string SoulstormLocation { get; }
    }
}
