using DowUmg.Constants;
using DowUmg.Data;
using DowUmg.Data.Entities;
using DowUmg.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : RoutableReactiveObject
    {
        public GenerationViewModel(IScreen screen, DowModLoader? modService = null) : base(screen, "matchup")
        {
            GameTab = new GameTabViewModel();
            TeamTab = new TeamTabViewModel();

            using var store = new ModsDataStore();

            DowMod[] mods = store.GetPlayableMods().ToArray();

            IEnumerable<DowMod> addonMods = mods.Where(mod => !mod.IsVanilla && DowConstants.IsVanilla(mod.ModFolder));
            IEnumerable<DowMod> baseMods = mods.Where(mod => mod.IsVanilla || !DowConstants.IsVanilla(mod.ModFolder));

            GeneralTab = new GeneralTabViewModel(addonMods.SelectMany(mod => mod.Maps).ToList());

            Mod = new OptionInputViewModel<DowMod>(mod => mod.Name, baseMods.ToArray());

            RefreshMod = ReactiveCommand.Create((DowMod mod) =>
            {
                GeneralTab.Mod = mod;
            });

            this.WhenAnyValue(x => x.Mod.SelectedItem)
                .DistinctUntilChanged()
                .Select(mod => mod.Item)
                .InvokeCommand(RefreshMod);
        }

        public GeneralTabViewModel GeneralTab { get; }

        public GameTabViewModel GameTab { get; }

        public TeamTabViewModel TeamTab { get; }

        public OptionInputViewModel<DowMod> Mod { get; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }
    }
}
