using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for TeamTabControl.xaml
    /// </summary>
    public partial class TeamTabControl : ReactiveUserControl<TeamTabViewModel>
    {
        public TeamTabControl()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.Players, v => v.PlayerItems.ItemsSource)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.RacesViewModel, v => v.Races.ViewModel)
                    .DisposeWith(d);

                this.Bind(
                        ViewModel,
                        vm => vm.RandomPositions,
                        v => v.RandomPositionsToggle.IsChecked
                    )
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.OneRaceTeams, v => v.OneRaceTeamToggle.IsChecked)
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.ViewModel)
                    .Do(vm =>
                    {
                        PlayersOption.ViewModel = vm.PlayerCountInput;
                        MinComputersOption.ViewModel = vm.MinComputers;
                        MaxComputersOption.ViewModel = vm.MaxComputers;
                        MinTeamsOption.ViewModel = vm.MinTeams;
                        MaxTeamsOption.ViewModel = vm.MaxTeams;
                    })
                    .Subscribe()
                    .DisposeWith(d);
            });
        }
    }
}
