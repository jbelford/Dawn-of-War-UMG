using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MatchupPlayerView.xaml
    /// </summary>
    public partial class MatchupPlayerView : ReactiveUserControl<MatchupPlayerViewModel>
    {
        public MatchupPlayerView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .Do(vm =>
                    {
                        RaceImage.Source = RaceBitMap.GetBitmapSource(vm.Race ?? "");
                        PositionText.Text = vm.Position.ToString();
                        PlayerNameText.Text = vm.Name;
                        TeamText.Text = vm.Team;

                        if (vm.Race != null)
                        {
                            RaceText.Text = vm.Race;
                        }
                        else
                        {
                            RaceText.Visibility = System.Windows.Visibility.Collapsed;
                        }

                        var teamColor =
                            new BrushConverter().ConvertFromString(vm.TeamColor) as Brush;
                        PositionText.Foreground = teamColor;
                        PlayerNameText.Foreground = teamColor;
                        TeamText.Foreground = teamColor;
                        RaceText.Foreground = teamColor;
                    })
                    .Subscribe()
                    .DisposeWith(d);
            });
        }
    }
}
