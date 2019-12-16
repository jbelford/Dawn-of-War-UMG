using Splat;

namespace DowUmg.Services
{
    public class GenerationService
    {
        private readonly DowModService modService;

        public GenerationService(DowModService? modService = null)
        {
            this.modService = modService ?? Locator.Current.GetService<DowModService>();
        }
    }
}
