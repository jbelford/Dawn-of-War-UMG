using DowUmg.Data;
using DowUmg.Data.Entities;
using DowUmg.Services;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : RoutableReactiveObject
    {
        public GenerationViewModel(IScreen screen, DowModLoader? modService = null) : base(screen, "matchup")
        {
            GeneralTab = new GeneralTabViewModel();
            GameTab = new GameTabViewModel();

            using var store = new ModsDataStore();

            DowMod[] mods = store.GetPlayableMods().ToArray();

            Mod = new OptionInputViewModel<DowMod>(mod => mod.Name, mods);

            RefreshMod = ReactiveCommand.Create((DowMod mod) =>
            {
                GeneralTab.Mod = mod;
            });

            this.WhenAnyValue(x => x.Mod.SelectedItem)
                .DistinctUntilChanged()
                .Select(mod => mod.Content)
                .InvokeCommand(RefreshMod);
        }

        public GeneralTabViewModel GeneralTab { get; }

        public GameTabViewModel GameTab { get; }

        public OptionInputViewModel<DowMod> Mod { get; }

        public ReactiveCommand<DowMod, Unit> RefreshMod { get; }
    }
}
