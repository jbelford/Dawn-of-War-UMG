using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowUmg.Data.Entities;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MapSelectionViewModel : ReactiveObject
    {
        public IObservableCollection<MapItemViewModel> Maps { get; set; }

        [Reactive]
        public MapItemViewModel SelectedMap { get; set; }
    }

    public class MapItemViewModel : MapListItemViewModel
    {
        public MapItemViewModel(DowMap map)
        {
            Map = map;

            MapImage = map.Image;
            Header = map.Name;
            Details = map.Details;
            Footer = $"Players: {map.Players} - Size: {map.Size}";
        }

        public DowMap Map { get; set; }
    }
}
