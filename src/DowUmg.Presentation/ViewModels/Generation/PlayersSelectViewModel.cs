using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class PlayersSelectViewModel : ReactiveObject
    {
        public PlayersSelectViewModel(string label, OptionInputViewModel<int> humans, RangeViewModel minMax)
        {
            Label = label;
            Humans = humans;
            MinMax = minMax;

            RefreshForHumanPlayers = ReactiveCommand.Create((OptionInputItem<int> item) =>
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
            });

            this.WhenAnyValue(x => x.Humans.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForHumanPlayers);
        }

        public string Label { get; }

        public OptionInputViewModel<int> Humans { get; }

        public RangeViewModel MinMax { get; }

        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForHumanPlayers { get; }
    }
}
