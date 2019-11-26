using DowUmg.Data;
using DowUmg.Services;
using Microsoft.EntityFrameworkCore;
using Splat;

namespace DowUmg
{
    public class AppBootstrapper
    {
        public static void RegisterDefaults()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new DataLoader());
            Locator.CurrentMutable.RegisterLazySingleton(() => new AppSettingsService());
            Locator.CurrentMutable.Register<DbContext>(() => new DataContext());
        }
    }
}
