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
using System.Threading.Tasks;

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

            var canLoad = this.WhenAnyValue(x => x.Loading, x => x.SelectedBaseItem, x => x.SelectedModItem,
                    (loading, baseItem, modItem) => !loading && (baseItem != null || modItem != null))
                .DistinctUntilChanged();

            this.dowModService = dowModService ?? Locator.Current.GetService<DowModService>();

            RefreshMods = ReactiveCommand.CreateFromTask(GetModsAsync);
            ReloadMod = ReactiveCommand.CreateFromTask<Unit, DowMod>(LoadAsync, canLoad);

            RefreshMods.ThrownExceptions.Subscribe(exception =>
            {
                this.logger.Error(exception);
            });

            ReloadMod.IsExecuting.DistinctUntilChanged()
                .ToPropertyEx(this, x => x.Loading, false);
        }

        public ReactiveCommand<Unit, Unit> RefreshMods { get; }

        public ReactiveCommand<Unit, DowMod> ReloadMod { get; }

        public ReactiveCommand<Unit, Unit> ReloadMods { get; }

        public string UrlPathSegment => "mods";

        public IScreen HostScreen { get; }

        [Reactive]
        public IList<ModItemViewModel> ModItems { get; private set; }

        [Reactive]
        public IList<ModItemViewModel> BaseGameItems { get; private set; }

        [Reactive]
        public ModItemViewModel? SelectedBaseItem { get; set; }

        [Reactive]
        public ModItemViewModel? SelectedModItem { get; set; }

        public extern bool Loading { [ObservableAsProperty] get; }

        public async Task GetModsAsync()
        {
            IList<ModItemViewModel> mods = await Observable.Start(() =>
                {
                    List<DowMod> loaded = this.dowModService.GetLoadedMods();
                    return this.dowModService.GetUnloadedMods()
                            .Select(mod => new ModItemViewModel()
                            {
                                Module = mod.File,
                                Locales = mod.Locales,
                                IsLoaded = loaded.Exists(loaded => mod.File.ModFolder.Equals(loaded.ModFolder))
                            }).ToList();
                }, RxApp.TaskpoolScheduler);

            ModItems = mods.Where(x => x.Module.Playable).Where(x => IsMod(x.Module.ModFolder.ToLower())).ToList();
            BaseGameItems = mods.Where(x => !IsMod(x.Module.ModFolder.ToLower())).ToList();
        }

        public async Task<DowMod> LoadAsync(Unit arg)
        {
            ModItemViewModel modItem = SelectedBaseItem ?? SelectedModItem!;
            DowMod mod = await Observable.Start(() =>
                {
                    var unloaded = new UnloadedMod() { File = modItem.Module, Locales = modItem.Locales };
                    return IsMod(modItem.Module.ModFolder.ToLower())
                        ? this.dowModService.LoadMod(unloaded)
                        : this.dowModService.LoadModArchive(unloaded);
                }, RxApp.TaskpoolScheduler);

            modItem.IsLoaded = true;
            return mod;
        }

        private static bool IsMod(string str) => !"dxp2".Equals(str) && !"w40k".Equals(str);
    }
}
