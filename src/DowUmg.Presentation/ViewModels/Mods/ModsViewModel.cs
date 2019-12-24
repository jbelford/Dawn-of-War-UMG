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

            foreach (var item in BaseGameItems.Concat(ModItems))
            {
                DowMod mod = await Observable.Start(() =>
                        dowModService.LoadMod(item.Module, allUnloaded, memo), RxApp.TaskpoolScheduler);

                store.Add(mod);

                item.IsLoaded = true;
            }
        }
    }
}
