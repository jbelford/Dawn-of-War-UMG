using DowUmg.Data;
using DowUmg.Data.Entities;
using DowUmg.FileFormats;
using Splat;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace DowUmg.Services
{
    public class DowModService
    {
        private readonly ModuleService moduleService;

        public DowModService(ModuleService? moduleService = null)
        {
            this.moduleService = moduleService ?? Locator.Current.GetService<ModuleService>();
        }

        public List<DowMod> GetLoadedMods()
        {
            using var context = new DataContext();
            return context.Mods.ToList();
        }

        public IEnumerable<UnloadedMod> GetUnloadedMods()
        {
            return this.moduleService.GetAllModules()
                .Where(x => x.Playable || "w40k".Equals(x.ModFolder.ToLower()))
                .Select(module =>
                {
                    Locales? locales = this.moduleService.GetLocales(module);
                    if (locales != null)
                    {
                        module.UIName = locales.Replace(module.UIName);
                        module.Description = locales.Replace(module.Description);
                    }
                    return new UnloadedMod() { File = module, Locales = locales };
                });
        }

        //public async Task<DowMod> LoadMod(UnloadedMod unloaded)
        //{
        //}

        public class UnloadedMod
        {
            public DowModuleFile File { get; set; } = null!;
            public Locales? Locales { get; set; }
        }
    }
}
