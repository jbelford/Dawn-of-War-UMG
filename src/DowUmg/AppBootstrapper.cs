using DowUmg.Platform;
using DowUmg.Services;
using Splat;

namespace DowUmg
{
    public class AppBootstrapper
    {
        public static void RegisterDefaults()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new AppSettingsService());
            Locator.CurrentMutable.RegisterLazySingleton(() => new DowModLoader());
            Locator.CurrentMutable.RegisterLazySingleton(() => new GenerationService());
            Locator.CurrentMutable.RegisterLazySingleton<IModDataService>(
                () => new ModDataService()
            );
            Locator.CurrentMutable.RegisterLazySingleton<ICampaignService>(
                () => new CampaignService()
            );

            var filePathProvider = Locator.Current.GetService<IFilePathProvider>();
            var appSettingsService = Locator.Current.GetService<AppSettingsService>();
            var settings = appSettingsService.GetSettings();
            if (settings.InstallLocation != null)
            {
                filePathProvider.SoulstormLocation = settings.InstallLocation;
            }
        }
    }
}
