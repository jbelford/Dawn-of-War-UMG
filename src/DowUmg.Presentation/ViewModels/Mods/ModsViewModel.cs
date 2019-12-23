﻿using DowUmg.Data.Entities;
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
        private readonly DowModService dowModService;

        public ModsViewModel(IScreen screen, DowModService? dowModService = null) : base(screen, "mods")
        {
            this.logger = this.Log();
            this.dowModService = dowModService ?? Locator.Current.GetService<DowModService>();

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
                    List<DowMod> mods = dowModService.GetLoadedMods();
                    return dowModService.GetUnloadedMods()
                            .Select(unloaded => new ModItemViewModel()
                            {
                                Module = unloaded,
                                IsLoaded = mods.Exists(mod => unloaded.File.ModFolder.Equals(mod.ModFolder)
                                    && mod.IsVanilla == unloaded.File.IsVanilla)
                            }).ToList();
                }, RxApp.TaskpoolScheduler);
        }

        private async Task LoadAllMods()
        {
            Dictionary<string, UnloadedMod> allUnloaded = BaseGameItems.Concat(ModItems)
                .GroupBy(item => item.Module.File.ModFolder, item => item.Module)
                .ToDictionary(g => g.Key, g => g.First());

            this.dowModService.RemoveLoadedMods();

            var allLoaded = new Dictionary<string, (DowMod?, DowMod?)>();

            foreach (var item in BaseGameItems.Concat(ModItems))
            {
                await LoadModAsync(item, allUnloaded, allLoaded);
            }
        }

        private async Task<DowMod> LoadModAsync(ModItemViewModel modItem, Dictionary<string, UnloadedMod> allUnloaded,
            Dictionary<string, (DowMod?, DowMod?)> allLoaded)
        {
            DowMod mod = await Observable.Start(() =>
            {
                return dowModService.LoadMod(modItem.Module, allUnloaded, allLoaded);
            }, RxApp.TaskpoolScheduler);

            modItem.IsLoaded = true;
            return mod;
        }
    }
}
