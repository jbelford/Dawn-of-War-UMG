using DowUmg.Interfaces;
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
            Locator.CurrentMutable.RegisterLazySingleton<ICampaignService>(
                () => new CampaignService()
            );
        }
    }
}
