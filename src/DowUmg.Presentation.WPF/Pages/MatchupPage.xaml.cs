using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for MatchupPage.xaml
    /// </summary>
    public partial class MatchupPage : ReactiveUserControl<MatchupViewModel>
    {
        public MatchupPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.ModName.Text,
                        m => m.Map.Mod.Name
                    )
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.MapName.Text, m => m.Map.Name)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.MapDesc.Text,
                        m => m.Map.Details
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.WinConditions.ItemsSource,
                        m => new ObservableCollection<string>(
                            m.GameInfo.Rules.Select(rule => rule.Name)
                        )
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.Difficulty.Text,
                        m => m.GameInfo.Options.Difficulty.ToString()
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.GameSpeed.Text,
                        m => m.GameInfo.Options.Speed.ToString()
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.ResourceRate.Text,
                        m => m.GameInfo.Options.ResourceRate.ToString()
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Matchup,
                        v => v.StartingResources.Text,
                        m => m.GameInfo.Options.StartingResources.ToString()
                    )
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
