using DowUmg.Data.Entities;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Linq;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModel : RoutableReactiveObject
    {
        private readonly DowModService modService;

        public GenerationViewModel(IScreen screen, DowModService? modService = null) : base(screen, "matchup")
        {
            this.modService = modService ?? Locator.Current.GetService<DowModService>();

            GeneralTab = new GeneralTabViewModel();
            GameTab = new GameTabViewModel();

            DowMod[] mods = this.modService.GetLoadedMods()
                .Where(mod => mod.MapsCount > 0 || mod.RulesCount > 0 || mod.RacesCount > 0)
                .ToArray();

            Mod = new OptionInputViewModel<DowMod>(mod => mod.Name, mods);

            this.WhenAnyValue(x => x.Mod.SelectedItem)
                .DistinctUntilChanged()
                .Select(mod => mod.Content)
                .Select(this.modService.Load)
                .ToPropertyEx(this, x => x.LoadedMod);
        }

        public GeneralTabViewModel GeneralTab { get; }

        public GameTabViewModel GameTab { get; }

        public OptionInputViewModel<DowMod> Mod { get; }

        public extern DowMod LoadedMod { [ObservableAsProperty] get; }
    }
}
