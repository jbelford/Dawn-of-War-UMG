using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace DowUmg.Presentation.ViewModels
{
    public class GeneralTabViewModel : ReactiveObject
    {
        public GeneralTabViewModel()
        {
            HumanPlayers = 1;
            MinPlayers = 2;
            MaxPlayers = 8;

            foreach (var x in Enumerable.Range(2, 7))
            {
                MapTypes.Add(new CheckBoxItemModel() { IsChecked = true, Label = $"{x}p" });
            }

            MapSizes.Add(new CheckBoxItemModel() { IsChecked = true, Label = "129" });
            MapSizes.Add(new CheckBoxItemModel() { IsChecked = true, Label = "257" });
            MapSizes.Add(new CheckBoxItemModel() { IsChecked = true, Label = "513" });
            MapSizes.Add(new CheckBoxItemModel() { IsChecked = true, Label = "1025" });
        }

        [Reactive]
        public int HumanPlayers { get; set; }

        [Reactive]
        public int MinPlayers { get; set; }

        [Reactive]
        public int MaxPlayers { get; set; }

        public ObservableCollection<CheckBoxItemModel> MapTypes { get; } = new ObservableCollection<CheckBoxItemModel>();

        public ObservableCollection<CheckBoxItemModel> MapSizes { get; } = new ObservableCollection<CheckBoxItemModel>();
    }
}
