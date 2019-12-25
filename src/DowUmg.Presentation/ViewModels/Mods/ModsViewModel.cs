using DowUmg.Data;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using DynamicData.Binding;
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
    public class ModsViewModel : RoutableReactiveObject, IEnableLogger
    {
        private readonly IFullLogger logger;
        private readonly DowModLoader dowModService;

        public ModsViewModel(IScreen screen, DowModLoader? dowModService = null) : base(screen, "mods")
        {
            this.logger = this.Log();
            this.dowModService = dowModService ?? Locator.Current.GetService<DowModLoader>();

            var whenNotLoading = this.WhenAnyValue(x => x.Loading).Select(loading => !loading).DistinctUntilChanged();

            var whenItemSelected = this.WhenAnyValue(x => x.SelectedBaseItem, x => x.SelectedModItem,
                    (baseItem, modItem) => baseItem != null || modItem != null).DistinctUntilChanged();

            var canLoadSpecific = whenNotLoading.CombineLatest(whenItemSelected, (notLoading, itemSelected) => notLoading && itemSelected).DistinctUntilChanged();

            ReloadMods = ReactiveCommand.CreateFromTask(LoadAllMods, whenNotLoading);

            RefreshMods = ReactiveCommand.CreateFromTask(GetModsAsync);
            RefreshMods.ThrownExceptions.Subscribe(exception => logger.Error(exception));
            RefreshMods.Execute().Subscribe();

            RefreshMods.Select(mods => mods.Where(x => !x.Module.File.IsVanilla))
                .Subscribe(mods =>
                {
                    ModItems.Clear();
                    ModItems.AddRange(mods);
                });

            RefreshMods.Select(mods => mods.Where(x => x.Module.File.IsVanilla))
                .Subscribe(mods =>
                {
                    BaseGameItems.Clear();
                    BaseGameItems.AddRange(mods);
                });
        }

        public ReactiveCommand<Unit, IList<ModItemViewModel>> RefreshMods { get; }
        public ReactiveCommand<Unit, Unit> ReloadMods { get; }
        public IObservableCollection<ModItemViewModel> ModItems { get; } = new ObservableCollectionExtended<ModItemViewModel>();
        public IObservableCollection<ModItemViewModel> BaseGameItems { get; } = new ObservableCollectionExtended<ModItemViewModel>();

        [Reactive]
        public ModItemViewModel? SelectedBaseItem { get; set; }

        [Reactive]
        public ModItemViewModel? SelectedModItem { get; set; }

        public extern bool Loading { [ObservableAsProperty] get; }

        private async Task<IList<ModItemViewModel>> GetModsAsync()
        {
            return await Observable.Start(() =>
                {
                    using var store = new ModsDataStore();
                    return dowModService.GetUnloadedMods()
                            .Select(unloaded => new ModItemViewModel()
                            {
                                Module = unloaded,
                                IsLoaded = store.GetAll().Any(mod => unloaded.File.ModFolder.Equals(mod.ModFolder)
                                    && mod.IsVanilla == unloaded.File.IsVanilla)
                            }).ToList();
                }, RxApp.TaskpoolScheduler);
        }

        private async Task LoadAllMods()
        {
            using var store = new ModsDataStore();
            store.DropAll();

            Dictionary<string, UnloadedMod> allUnloaded = BaseGameItems.Concat(ModItems)
                .GroupBy(item => item.Module.File.ModFolder, item => item.Module)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var item in BaseGameItems.Concat(ModItems))
            {
                item.IsLoaded = false;
            }

            var memo = new LoadMemo();

            foreach (var item in BaseGameItems.Concat(ModItems).Where(mod => mod.Module.File.Playable))
            {
                DowMod mod = await Observable.Start(() =>
                        dowModService.LoadMod(item.Module, allUnloaded, memo), RxApp.TaskpoolScheduler);

                store.Add(mod);

                item.IsLoaded = true;
            }

            // Yeah this is kind of weird. But essentially we want to load the playable mods first.
            // This is because they likely depend on the non-playable mods. Non-playable mods may be
            // sub-modules for playable mods. As such they might have Locales that reference the parent mod
            // for this reason we want to ensure that we load from the root.

            foreach (var item in BaseGameItems.Concat(ModItems).Where(mod => !mod.Module.File.Playable))
            {
                DowMod mod = dowModService.LoadMod(item.Module, allUnloaded, memo);

                store.Add(mod);

                item.IsLoaded = true;
            }
        }
    }
}
