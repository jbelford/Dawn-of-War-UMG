using DowUmg.Data.Entities;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class ModsViewModel : ReactiveObject, IRoutableViewModel, IEnableLogger
    {
        private readonly IFullLogger logger;
        private readonly DowModService dowModService;

        public ModsViewModel(RoutingViewModel routing, DowModService? dowModService = null)
        {
            HostScreen = routing;

            this.logger = this.Log();

            this.dowModService = dowModService ?? Locator.Current.GetService<DowModService>();

            RefreshMods = ReactiveCommand.CreateFromTask(async () =>
            {
                List<DowMod> loaded = this.dowModService.GetLoadedMods();
                IList<ModItemViewModel> mods = await this.dowModService.GetUnloadedMods()
                    .Select(mod => new ModItemViewModel()
                    {
                        Module = mod.File,
                        IsLoaded = loaded.Exists(loaded => mod.File.ModFolder.Equals(loaded.Path))
                    })
                    .ToList()
                    .SubscribeOn(RxApp.TaskpoolScheduler);

                static bool IsMod(string str) => !"dxp2".Equals(str) && !"w40k".Equals(str);

                ModItems = mods.Where(x => IsMod(x.Module.ModFolder.ToLower())).ToList();
                BaseGameItems = mods.Where(x => !IsMod(x.Module.ModFolder.ToLower())).ToList();
            });

            RefreshMods.ThrownExceptions.Subscribe(exception =>
            {
                this.logger.Error(exception);
            });
        }

        public ReactiveCommand<Unit, Unit> RefreshMods { get; }
        public ReactiveCommand<Unit, Unit> ReloadMod { get; }
        public ReactiveCommand<Unit, Unit> ReloadMods { get; }
        public string UrlPathSegment => "mods";
        public IScreen HostScreen { get; }

        [Reactive]
        public IList<ModItemViewModel> ModItems { get; private set; }

        [Reactive]
        public IList<ModItemViewModel> BaseGameItems { get; private set; }
    }
}
