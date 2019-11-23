namespace DowUmg.Services.Interfaces
{
    public interface IAppDataProvider
    {
        string AppDataLocation { get; }

        string SettingsLocation { get; }

        string DataLocation { get; }
    }
}
