using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MatchupPage.xaml
    /// </summary>
    public partial class MatchupView : ReactiveUserControl<MatchupViewModel>
    {
        public MatchupView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel.Matchup)
                    .Do(matchup =>
                    {
                        ModName.Text = $"Mod Folder: {matchup.Map.Mod.ModFolder}";
                        MapName.Text = matchup.Map.Name;
                        MapDesc.Text = matchup.Map.Details;

                        WinConditions.ItemsSource = new ObservableCollection<string>(
                            matchup.GameInfo.Rules.Select(rule => rule.Name)
                        );

                        Difficulty.Text = matchup.GameInfo.Options.Difficulty.ToString();
                        GameSpeed.Text = matchup.GameInfo.Options.Speed.ToString();
                        ResourceRate.Text = matchup.GameInfo.Options.ResourceRate.ToString();
                        StartingResources.Text =
                            matchup.GameInfo.Options.StartingResources.ToString();
                    })
                    .Subscribe()
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.MatchupPlayers, v => v.Players.ItemsSource)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.MapImagePath,
                        v => v.MapImage.Source,
                        vmToViewConverterOverride: new MapPathToSourceConverter()
                    )
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GenerateMatchup, v => v.RegenerateButton)
                    .DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.GoBack, v => v.BackButton).DisposeWith(d);
            });
        }
    }
}
