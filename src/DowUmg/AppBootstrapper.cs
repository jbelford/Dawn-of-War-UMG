using DowUmg.Services;
using Splat;

namespace DowUmg
{
    public class AppBootstrapper
    {
        public static void RegisterDefaults()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new AppSettingsService());
        }
    }
}
