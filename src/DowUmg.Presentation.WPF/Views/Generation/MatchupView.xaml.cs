using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Constants;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;
using Wpf.Ui.Controls;

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

                        Difficulty.Text = matchup.GameInfo.Options.Difficulty?.GetName();
                        GameSpeed.Text = matchup.GameInfo.Options.Speed?.GetName();
                        ResourceRate.Text = matchup.GameInfo.Options.ResourceRate?.GetName();
                        StartingResources.Text =
                            matchup.GameInfo.Options.StartingResources?.GetName();
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

                this.BindCommand(ViewModel, vm => vm.CollapsePlayers, v => v.CollapseButton)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.ImageVisible,
                        v => v.CollapseSymbol.Symbol,
                        visible =>
                            visible ? SymbolRegular.ArrowMinimize24 : SymbolRegular.ArrowExpand24
                    )
                    .DisposeWith(d);
            });
        }
    }
}
