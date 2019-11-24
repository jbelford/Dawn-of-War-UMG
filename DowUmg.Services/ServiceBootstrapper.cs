using DowUmg.Services.Data;
using Microsoft.EntityFrameworkCore;
using Splat;

namespace DowUmg.Services
{
    public class ServiceBootstrapper
    {
        public static void RegisterDefaults()
        {
            Locator.CurrentMutable.RegisterLazySingleton(() => new DataLoader());
            Locator.CurrentMutable.RegisterLazySingleton(() => new AppSettingsService());
            Locator.CurrentMutable.Register<DbContext>(() => new DataContext());
        }
    }
}
