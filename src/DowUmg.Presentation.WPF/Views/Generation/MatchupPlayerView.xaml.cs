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
        private static string[] TeamColours =
        [
            "#DDDDDD",
            "#44BB99",
            "#77AADD",
            "#EEDD88",
            "#FFAABB",
            "#EE8866",
            "#BBCC33",
            "#99DDFF",
        ];

        public MatchupPlayerView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .Do(vm =>
                    {
                        RaceImage.Source = RaceNameToSourceConverter.GetBitmapSource(vm.Race ?? "");
                        PositionText.Text = vm.Position.ToString();
                        PlayerNameText.Text = vm.Name;
                        TeamText.Text = $"Team {vm.Team + 1}";

                        if (vm.Race != null)
                        {
                            RaceText.Text = vm.Race;
                        }
                        else
                        {
                            RaceText.Visibility = System.Windows.Visibility.Collapsed;
                        }

                        var teamColor =
                            new BrushConverter().ConvertFromString(TeamColours[vm.Team]) as Brush;
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
