using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class PlayersSelectViewModel : ReactiveObject
    {
        public PlayersSelectViewModel(string label, OptionInputViewModel<int> humans, RangeViewModel minMax)
        {
            Label = label;
            Humans = humans;
            MinMax = minMax;
        }

        public string Label { get; }

        public OptionInputViewModel<int> Humans { get; }

        public RangeViewModel MinMax { get; }
    }
}
