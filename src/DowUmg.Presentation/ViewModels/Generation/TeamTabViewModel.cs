using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DowUmg.Data.Entities;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class TeamTabViewModel : ActivatableReactiveObject
    {
        public TeamTabViewModel(GenerationViewModelState generationState)
        {
            var totalPlayers = Enumerable.Range(1, 8);

            PlayerCountInput = new OptionInputViewModel(
                totalPlayers.Select(name => new OptionInputItemViewModel($"{name}p", name)).ToList()
            );

            MinComputers = new OptionInputViewModel(
                totalPlayers.Select(name => new OptionInputItemViewModel($"{name}p", name)).ToList()
            );
            MaxComputers = new OptionInputViewModel(
                totalPlayers
                    .Select(name => new OptionInputItemViewModel($"{name}p", name))
                    .ToList(),
                true
            );

            var totalTeams = Enumerable.Range(1, 7);
            MinTeams = new OptionInputViewModel(
                totalTeams.Select(name => new OptionInputItemViewModel($"{name}", name)).ToList()
            );
            MaxTeams = new OptionInputViewModel(
                totalTeams.Select(name => new OptionInputItemViewModel($"{name}", name)).ToList(),
                true
            );

            generationState
                .ConnectRaces()
                .Transform(race => new ToggleModel<DowRace>(
                    race,
                    new ToggleItemViewModel(race.Name)
                ))
                .BindToObservableList(out _race);

            RacesViewModel = new ToggleItemListViewModel(
                "Races",
                _race.Connect().Transform(model => model.ToggleItem)
            );

            var players = totalPlayers
                .Select(idx => new TeamTabPlayerViewModel($"Player {idx}"))
                .ToList();

            this.WhenAnyValue(x => x.PlayerCountInput.SelectedItem)
                .Select(item => item.GetItem<int>())
                .Select(numPlayers => players[0..numPlayers])
                .Select(items => new ObservableCollection<TeamTabPlayerViewModel>(items))
                .ToPropertyEx(this, x => x.Players);
        }

        [Reactive]
        public OptionInputViewModel PlayerCountInput { get; set; }

        [ObservableAsProperty]
        public ObservableCollection<TeamTabPlayerViewModel> Players { get; set; }

        private IObservableList<ToggleModel<DowRace>> _race;
        internal IObservableList<ToggleModel<DowRace>> Races => _race;
        public ToggleItemListViewModel RacesViewModel { get; set; }

        [Reactive]
        public OptionInputViewModel MinComputers { get; set; }

        [Reactive]
        public OptionInputViewModel MaxComputers { get; set; }

        [Reactive]
        public OptionInputViewModel MinTeams { get; set; }

        [Reactive]
        public OptionInputViewModel MaxTeams { get; set; }

        [Reactive]
        public bool RandomPositions { get; set; }

        [Reactive]
        public bool OneRaceTeams { get; set; }
    }

    public class TeamTabPlayerViewModel : ReactiveObject
    {
        public TeamTabPlayerViewModel(string name)
        {
            Name = name;
        }

        [Reactive]
        public string Name { get; set; }
    }
}
