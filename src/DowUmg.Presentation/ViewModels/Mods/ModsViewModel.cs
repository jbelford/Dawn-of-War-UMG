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
    public class ModsViewModel : ReactiveObject, IRoutableViewModel, IEnableLogger
    {
        private readonly IFullLogger logger;
        private readonly DowModService dowModService;

        public ModsViewModel(IScreen screen, DowModService? dowModService = null)
        {
            HostScreen = screen;

            logger = this.Log();

            var whenNotLoading = this.WhenAnyValue(x => x.Loading).Select(loading => !loading).DistinctUntilChanged();

            var whenItemSelected = this.WhenAnyValue(x => x.SelectedBaseItem, x => x.SelectedModItem,
                    (baseItem, modItem) => baseItem != null || modItem != null).DistinctUntilChanged();

            var canLoadSpecific = whenNotLoading.CombineLatest(whenItemSelected, (notLoading, itemSelected) => notLoading && itemSelected).DistinctUntilChanged();

            this.dowModService = dowModService ?? Locator.Current.GetService<DowModService>();

            ReloadMod = ReactiveCommand.CreateFromTask(async () => await LoadModAsync(SelectedBaseItem ?? SelectedModItem!), canLoadSpecific);
            ReloadMods = ReactiveCommand.CreateFromTask(LoadAllMods, whenNotLoading);

            RefreshMods = ReactiveCommand.CreateFromTask(GetModsAsync);
            RefreshMods.ThrownExceptions.Subscribe(exception =>
            {
                logger.Error(exception);
            });
            RefreshMods.Execute().Subscribe();

            ReloadMod.IsExecuting.CombineLatest(ReloadMods.IsExecuting, (a, b) => a || b)
                .ToPropertyEx(this, x => x.Loading, false);

            RefreshMods.Select(mods => mods.Where(x => x.Module.Playable).Where(x => IsMod(x.Module.ModFolder.ToLower())))
                .Subscribe(mods =>
                {
                    _ModItemSource.Clear();
                    _ModItemSource.AddRange(mods);
                });

            RefreshMods.Select(mods => mods.Where(x => !IsMod(x.Module.ModFolder.ToLower())))
                .Subscribe(mods =>
                {
                    _BaseItemSource.Clear();
                    _BaseItemSource.AddRange(mods);
                });

            _ModItemSource.Connect()
                .Bind(ModItems)
                .Subscribe();

            _BaseItemSource.Connect()
                .Bind(BaseGameItems)
                .Subscribe();
        }

        public ReactiveCommand<Unit, IList<ModItemViewModel>> RefreshMods { get; }
        public ReactiveCommand<Unit, DowMod> ReloadMod { get; }
        public ReactiveCommand<Unit, Unit> ReloadMods { get; }
        public string UrlPathSegment => "mods";
        public IScreen HostScreen { get; }
        public IObservableCollection<ModItemViewModel> ModItems { get; } = new ObservableCollectionExtended<ModItemViewModel>();
        public IObservableCollection<ModItemViewModel> BaseGameItems { get; } = new ObservableCollectionExtended<ModItemViewModel>();

        [Reactive]
        public ModItemViewModel? SelectedBaseItem { get; set; }

        [Reactive]
        public ModItemViewModel? SelectedModItem { get; set; }

        public extern bool Loading { [ObservableAsProperty] get; }
        private ISourceList<ModItemViewModel> _ModItemSource { get; } = new SourceList<ModItemViewModel>();
        private ISourceList<ModItemViewModel> _BaseItemSource { get; } = new SourceList<ModItemViewModel>();

        private static bool IsMod(string str) => !"dxp2".Equals(str) && !"w40k".Equals(str);

        private async Task<IList<ModItemViewModel>> GetModsAsync()
        {
            return await Observable.Start(() =>
                {
                    List<DowMod> loaded = dowModService.GetLoadedMods();
                    return dowModService.GetUnloadedMods()
                            .Select(mod => new ModItemViewModel()
                            {
                                Module = mod.File,
                                Locales = mod.Locales,
                                IsLoaded = loaded.Exists(loaded => mod.File.ModFolder.Equals(loaded.ModFolder))
                            }).ToList();
                }, RxApp.TaskpoolScheduler);
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
                return dowModService.LoadMod(unloaded);
            }, RxApp.TaskpoolScheduler);

            modItem.IsLoaded = true;
            return mod;
        }
    }
}
