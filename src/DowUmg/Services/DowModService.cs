using DowUmg.Data;
using DowUmg.Data.Entities;
using DowUmg.FileFormats;
using Splat;
using System;
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

        public IObservable<UnloadedMod> GetUnloadedMods()
        {
            return this.moduleService.GetAllModules()
                .Where(x => x.Playable || "w40k".Equals(x.ModFolder.ToLower()))
                .Select(module => new UnloadedMod() { File = module, Locales = this.moduleService.GetLocales(module) })
                .Do(unloaded =>
                {
                    if (unloaded.Locales != null)
                    {
                        unloaded.File.UIName = unloaded.Locales.Replace(unloaded.File.UIName);
                        unloaded.File.Description = unloaded.Locales.Replace(unloaded.File.Description);
                    }
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
