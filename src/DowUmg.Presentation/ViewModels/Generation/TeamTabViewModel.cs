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
        public TeamTabViewModel(ModGenerationState modState)
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

            modState
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
                .DistinctUntilChanged()
                .Select(item => item.GetItem<int>())
                .Select(numPlayers => players[0..numPlayers])
                .Select(items => new ObservableCollection<TeamTabPlayerViewModel>(items))
                .ToPropertyEx(this, x => x.Players);

            this.WhenAnyValue(x => x.PlayerCountInput.SelectedItem)
                .DistinctUntilChanged()
                .Subscribe(players =>
                {
                    UpdateMinComputers(
                        players.GetItem<int>(),
                        MinTeams.SelectedItem.GetItem<int>()
                    );
                });

            this.WhenAnyValue(x => x.MinComputers.SelectedItem)
                .DistinctUntilChanged()
                .Subscribe(minComputers =>
                {
                    UpdateMaxComputers(
                        minComputers.GetItem<int>(),
                        PlayerCountInput.SelectedItem.GetItem<int>()
                    );
                });

            this.WhenAnyValue(x => x.MinTeams.SelectedItem)
                .DistinctUntilChanged()
                .Subscribe(minTeams =>
                {
                    UpdateMinPlayers(minTeams.GetItem<int>());
                });
        }

        private void UpdateMinPlayers(int minTeams)
        {
            for (int i = 0; i < 7; ++i)
            {
                PlayerCountInput.Items[i].IsEnabled = i < 8 - minTeams;
                MaxTeams.Items[i].IsEnabled = i + 1 >= minTeams;
            }
            if (!MaxTeams.SelectedItem.IsEnabled)
            {
                MaxTeams.SelectedItem = MaxTeams.Items[minTeams - 1];
            }
            if (!PlayerCountInput.SelectedItem.IsEnabled)
            {
                var lastItem = PlayerCountInput.Items.Where(item => item.IsEnabled).Last();
                PlayerCountInput.SelectedItem = lastItem;
            }
            else
            {
                UpdateMinComputers(PlayerCountInput.SelectedItem.GetItem<int>(), minTeams);
            }
        }

        private void UpdateMinComputers(int players, int minTeams)
        {
            for (int i = 0; i < 7; ++i)
            {
                MinComputers.Items[i].IsEnabled = i + players < 8 && i + 1 >= minTeams;
            }
            if (!MinComputers.SelectedItem.IsEnabled)
            {
                MinComputers.SelectedItem = MinComputers
                    .Items.Where(item => item.IsEnabled)
                    .First();
            }
            else
            {
                UpdateMaxComputers(MinComputers.SelectedItem.GetItem<int>(), players);
            }
        }

        private void UpdateMaxComputers(int minComputers, int playerCount)
        {
            for (int i = 0; i < 7; ++i)
            {
                MaxComputers.Items[i].IsEnabled = i + 1 >= minComputers && i + playerCount < 8;
            }
            if (!MaxComputers.SelectedItem.IsEnabled)
            {
                MaxComputers.SelectedItem = MaxComputers.Items.Where(item => item.IsEnabled).Last();
            }
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
        public bool EvenTeams { get; set; }

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
