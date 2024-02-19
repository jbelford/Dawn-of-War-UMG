using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Data.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class PlayersSelectViewModel : ActivatableReactiveObject
    {
        public PlayersSelectViewModel(string label, IEnumerable<int> humans, RangeViewModel minMax)
        {
            Label = label;
            Humans = new OptionInputViewModel<int>(humans);
            MinMax = minMax;

            RefreshForHumanPlayers = ReactiveCommand.Create(
                (OptionInputItem<int> item) =>
                {
                    OptionInputViewModel<int> minInput = MinMax.MinInput;
                    foreach (var minItem in minInput.Items)
                    {
                        minItem.IsEnabled = minItem.Item >= item.Item;
                    }
                    if (!minInput.SelectedItem.IsEnabled)
                    {
                        minInput.SelectedItem = minInput.Items.Where(x => x.IsEnabled).First();
                    }
                }
            );

            RefreshForRaces = ReactiveCommand.Create(
                (IEnumerable<DowRace> races) =>
                {
                    Races = new ProportionalOptionsViewModel<DowRace>(
                        "Races",
                        race => race.Name,
                        races.ToArray()
                    );
                }
            );

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.Humans.SelectedItem)
                    .DistinctUntilChanged()
                    .InvokeCommand(RefreshForHumanPlayers)
                    .DisposeWith(d);
            });
        }

        public string Label { get; }

        public OptionInputViewModel<int> Humans { get; }

        public RangeViewModel MinMax { get; }

        [Reactive]
        public ProportionalOptionsViewModel<DowRace> Races { get; set; }

        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForHumanPlayers { get; }

        public ReactiveCommand<IEnumerable<DowRace>, Unit> RefreshForRaces { get; }
    }
}
