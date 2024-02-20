using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class PlayersSelectViewModel : ActivatableReactiveObject
    {
        public PlayersSelectViewModel(string label, IEnumerable<int> humans, RangeViewModel minMax)
        {
            Label = label;
            MinMax = minMax;

            var sourceHumans = new SourceList<int>();
            sourceHumans.AddRange(humans);
            Humans = new OptionInputViewModel(
                sourceHumans
                    .Connect()
                    .Transform(human => new OptionInputItemViewModel(human.ToString(), human))
            );

            RefreshForHumanPlayers = ReactiveCommand.Create(
                (OptionInputItemViewModel item) =>
                {
                    OptionInputViewModel minInput = MinMax.MinInputViewModel;
                    foreach (var minItem in minInput.Items)
                    {
                        minItem.IsEnabled = minItem.GetItem<int>() >= item.GetItem<int>();
                    }
                    if (!minInput.SelectedItem.IsEnabled)
                    {
                        minInput.SelectedItem = minInput.Items.Where(x => x.IsEnabled).First();
                    }
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

        public OptionInputViewModel Humans { get; }

        public RangeViewModel MinMax { get; }

        public ReactiveCommand<OptionInputItemViewModel, Unit> RefreshForHumanPlayers { get; }
    }
}
