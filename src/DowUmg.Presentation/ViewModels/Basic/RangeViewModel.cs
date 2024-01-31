using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class RangeViewModel : ReactiveObject
    {
        public RangeViewModel(int min, int max)
        {
            int count = max - min + 1;
            MinInput = new OptionInputViewModel<int>(Enumerable.Range(min, count).ToArray());
            MaxInput = new OptionInputViewModel<int>(Enumerable.Range(min, count).ToArray());
            MaxInput.SelectedItem = MaxInput.Items.Last();

            RefreshForMin = ReactiveCommand.Create(
                (OptionInputItem<int> item) =>
                {
                    foreach (var maxItem in MaxInput.Items)
                    {
                        maxItem.IsEnabled = maxItem.Item >= item.Item;
                    }
                    if (!MaxInput.SelectedItem.IsEnabled)
                    {
                        MaxInput.SelectedItem = MaxInput.Items.Where(x => x.IsEnabled).First();
                    }
                }
            );

            this.WhenAnyValue(x => x.MinInput.SelectedItem)
                .DistinctUntilChanged()
                .InvokeCommand(RefreshForMin);
        }

        public OptionInputViewModel<int> MinInput { get; }
        public OptionInputViewModel<int> MaxInput { get; }
        public ReactiveCommand<OptionInputItem<int>, Unit> RefreshForMin { get; }
    }
}
