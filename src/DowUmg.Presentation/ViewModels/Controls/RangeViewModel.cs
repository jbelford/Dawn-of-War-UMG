using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class RangeViewModel : ActivatableReactiveObject
    {
        public RangeViewModel(int min, int max)
        {
            int count = max - min + 1;
            var inputItems = new SourceList<int>();
            inputItems.AddRange(Enumerable.Range(min, count));

            RefreshForMin = ReactiveCommand.Create(
                (OptionInputItemViewModel item) =>
                {
                    if (MaxInputViewModel.Items == null)
                    {
                        return;
                    }
                    var selected = item.GetItem<int>();
                    foreach (var maxItem in MaxInputViewModel.Items)
                    {
                        maxItem.IsEnabled = maxItem.GetItem<int>() >= selected;
                    }
                    if (!MaxInputViewModel.SelectedItem.IsEnabled)
                    {
                        MaxInputViewModel.SelectedItem = MaxInputViewModel
                            .Items.Where(x => x.IsEnabled)
                            .First();
                    }
                }
            );

            MinInputViewModel = new OptionInputViewModel(
                inputItems
                    .Connect()
                    .Transform(num => new OptionInputItemViewModel(num.ToString(), num))
            );
            MaxInputViewModel = new OptionInputViewModel(
                inputItems
                    .Connect()
                    .Transform(num => new OptionInputItemViewModel(num.ToString(), num)),
                true
            );

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.MinInputViewModel.SelectedItem)
                    .WhereNotNull()
                    .DistinctUntilChanged()
                    .InvokeCommand(RefreshForMin)
                    .DisposeWith(d);
            });
        }

        public OptionInputViewModel MinInputViewModel { get; set; }
        public OptionInputViewModel MaxInputViewModel { get; set; }

        public ReactiveCommand<OptionInputItemViewModel, Unit> RefreshForMin { get; }
    }
}
