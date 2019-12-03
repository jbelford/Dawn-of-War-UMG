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

            var whenNotLoading = this.WhenAnyValue(x => x.Loading).Select(loading => !loading).DistinctUntilChanged();

            var whenItemSelected = this.WhenAnyValue(x => x.SelectedBaseItem, x => x.SelectedModItem,
                    (baseItem, modItem) => baseItem != null || modItem != null).DistinctUntilChanged();

            var canLoadSpecific = whenNotLoading.CombineLatest(whenItemSelected, (notLoading, itemSelected) => notLoading && itemSelected).DistinctUntilChanged();

            this.dowModService = dowModService ?? Locator.Current.GetService<DowModService>();

            RefreshMods = ReactiveCommand.CreateFromTask(GetModsAsync);
            ReloadMod = ReactiveCommand.CreateFromTask(async () => await LoadModAsync(SelectedBaseItem ?? SelectedModItem!), canLoadSpecific);
            ReloadMods = ReactiveCommand.CreateFromTask(LoadAllMods, whenNotLoading);

            RefreshMods.ThrownExceptions.Subscribe(exception =>
            {
                this.logger.Error(exception);
            });

            Observable.CombineLatest(ReloadMod.IsExecuting, ReloadMods.IsExecuting, (a, b) => a || b)
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

        private static bool IsMod(string str) => !"dxp2".Equals(str) && !"w40k".Equals(str);

        private async Task GetModsAsync()
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

        private async Task LoadAllMods()
        {
            await Task.WhenAll(ModItems.Select(LoadModAsync).ToArray());
            await Task.WhenAll(BaseGameItems.Select(LoadModAsync).ToArray());
        }

        private async Task<DowMod> LoadModAsync(ModItemViewModel modItem)
        {
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
    }
}
