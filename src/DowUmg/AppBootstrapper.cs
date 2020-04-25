using DowUmg.Services;
using Splat;

namespace DowUmg
{
    public class AppBootstrapper
    {
        public static void RegisterDefaults()
        {
            Locator.CurrentMutable.Register(() => new AppSettingsService());
            Locator.CurrentMutable.Register(() => new DowModLoader());
            Locator.CurrentMutable.Register(() => new GenerationService());
        }
    }
}
