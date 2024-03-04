using System;
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
            var totalPlayers = Enumerable.Range(1, 7);
            var totalComputers = Enumerable.Range(1, 7);

            PlayerCountInput = new OptionInputViewModel(
                totalPlayers.Select(name => new OptionInputItemViewModel($"{name}p", name)).ToList()
            );

            MinComputers = new OptionInputViewModel(
                totalComputers
                    .Select(name => new OptionInputItemViewModel($"{name}p", name))
                    .ToList()
            );
            MaxComputers = new OptionInputViewModel(
                totalComputers
                    .Select(name => new OptionInputItemViewModel($"{name}p", name))
                    .ToList(),
                true
            );

            MinTeams = new OptionInputViewModel(
                totalComputers
                    .Select(name => new OptionInputItemViewModel($"{name}", name))
                    .ToList()
            );
            MaxTeams = new OptionInputViewModel(
                totalComputers
                    .Select(name => new OptionInputItemViewModel($"{name}", name))
                    .ToList(),
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

            this.WhenAnyValue(x => x.PlayerCountInput.SelectedItem)
                .DistinctUntilChanged()
                .Select(item => item.GetItem<int>())
                .Subscribe(players =>
                {
                    for (int i = 0; i < 7; ++i)
                    {
                        MinComputers.Items[i].IsEnabled = i + players < 8;
                        MaxComputers.Items[i].IsEnabled = i + players < 8;
                    }
                    if (!MinComputers.SelectedItem.IsEnabled)
                    {
                        MinComputers.SelectedItem = MinComputers.Items[0];
                    }
                    if (!MaxComputers.SelectedItem.IsEnabled)
                    {
                        MaxComputers.SelectedItem = MaxComputers.Items[7 - players];
                    }
                });
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
        public TeamTabPlayerViewModel(string label)
        {
            Label = label;
            Name = "";
        }

        [Reactive]
        public string Label { get; set; }

        [Reactive]
        public string Name { get; set; }
    }
}
