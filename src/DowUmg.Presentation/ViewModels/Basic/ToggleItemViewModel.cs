using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class ToggleItemViewModel<T> : ItemViewModel<T>
    {
        private readonly ObservableAsPropertyHelper<bool> _isShown;

        public ToggleItemViewModel()
        {
            IsToggled = true;
            IsEnabled = true;
            this.WhenAnyValue(x => x.IsEnabled, x => x.IsFiltered, (enabled, filtered) => enabled && !filtered)
                .DistinctUntilChanged()
                .ToProperty(this, x => x.IsShown, out _isShown);
        }

        [Reactive]
        public bool IsToggled { get; set; }

        [Reactive]
        public bool IsEnabled { get; set; }

        [Reactive]
        public bool IsFiltered { get; set; }

        public bool IsShown => _isShown.Value;
    }

    public class ToggleItemViewModel : ToggleItemViewModel<object>
    {
        public ToggleItemViewModel()
        {
        }
    }
}
