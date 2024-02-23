using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DowUmg.Constants;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class ModsViewModel : RoutableReactiveObject, IEnableLogger
    {
        private readonly IFullLogger logger;
        private readonly DowModLoader modLoader;
        private readonly IModDataService modDataService;

        public ModsViewModel(IScreen screen, DowModLoader? dowModService = null)
            : base(screen, "mods")
        {
            this.logger = this.Log();
            this.modLoader = dowModService ?? Locator.Current.GetService<DowModLoader>()!;
            this.modDataService = Locator.Current.GetService<IModDataService>()!;

            var whenNotLoading = this.WhenAnyValue(x => x.Loading)
                .Select(loading => !loading)
                .DistinctUntilChanged();

            ReloadMods = ReactiveCommand.CreateFromTask(LoadModsAsync, whenNotLoading);

            RefreshMods = ReactiveCommand.CreateFromTask(GetModsAsync);
            RefreshMods.ThrownExceptions.Subscribe(exception => logger.Error(exception));
            RefreshMods.Execute().Subscribe();

            RefreshMods
                .Select(mods => mods.Where(x => !x.Module.File.IsVanilla))
                .Subscribe(mods =>
                {
                    ModItems.Clear();
                    ModItems.AddRange(mods);
                });

            RefreshMods
                .Select(mods => mods.Where(x => x.Module.File.IsVanilla))
                .Subscribe(mods =>
                {
                    BaseGameItems.Clear();
                    BaseGameItems.AddRange(mods);
                });
        }

        public ReactiveCommand<Unit, IList<ModItemViewModel>> RefreshMods { get; }
        public ReactiveCommand<Unit, Unit> ReloadMods { get; }
        public IObservableCollection<ModItemViewModel> ModItems { get; } =
            new ObservableCollectionExtended<ModItemViewModel>();
        public IObservableCollection<ModItemViewModel> BaseGameItems { get; } =
            new ObservableCollectionExtended<ModItemViewModel>();

        public extern bool Loading
        {
            [ObservableAsProperty]
            get;
        }

        private async Task<IList<ModItemViewModel>> GetModsAsync()
        {
            return await Observable.Start(
                () =>
                {
                    IEnumerable<DowMod> mods = modDataService.GetMods();
                    return modLoader
                        .GetUnloadedMods()
                        .Select(unloaded => new ModItemViewModel()
                        {
                            Module = unloaded,
                            IsLoaded = mods.Any(mod =>
                                unloaded.File.FileName.Equals(mod.ModFile)
                                && mod.IsVanilla == unloaded.File.IsVanilla
                            )
                        })
                        .ToList();
                },
                RxApp.TaskpoolScheduler
            );
        }

        private async Task LoadModsAsync()
        {
            var allItems = BaseGameItems.Concat(ModItems);
            Dictionary<(string, bool), UnloadedMod> allUnloaded = allItems.ToDictionary(
                item => (item.Module.File.FileName, item.Module.File.IsVanilla),
                item => item.Module
            );

            foreach (var item in allItems)
            {
                item.IsLoaded = false;
            }

            var mods = new List<DowMod>();
            var memo = new LoadMemo();
            foreach (
                var item in allItems.Where(mod =>
                    mod.Module.File.Playable || DowConstants.IsVanilla(mod.Module.File.ModFolder)
                )
            )
            {
                DowMod mod = await Observable.Start(
                    () => modLoader.LoadMod(item.Module, allUnloaded, memo),
                    RxApp.TaskpoolScheduler
                );

                mods.Add(mod);

                item.IsLoaded = true;
            }

            foreach (var item in allItems.Where(mod => !mod.Module.File.Playable))
            {
                item.IsLoaded = memo.GetMod(item.Module.File) != null;
            }

            await Observable.Start(
                () =>
                {
                    modDataService.DropModData();
                    modDataService.Add(mods);
                },
                RxApp.TaskpoolScheduler
            );
        }
    }
}
