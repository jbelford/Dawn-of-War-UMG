using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.ModName.Text,
                    m => m.Map.Mod.Name).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.MapName.Text,
                    m => m.Map.Name).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.MapDesc.Text,
                    m => m.Map.Details).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.WinConditions.ItemsSource,
                    m => new ObservableCollection<string>(m.GameInfo.Rules.Select(rule => rule.Name))).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.Difficulty.Text,
                    m => m.GameInfo.Options.Difficulty.ToString()).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.GameSpeed.Text,
                    m => m.GameInfo.Options.Speed.ToString()).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.ResourceRate.Text,
                    m => m.GameInfo.Options.ResourceRate.ToString()).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Matchup, v => v.StartingResources.Text,
                    m => m.GameInfo.Options.StartingResources.ToString()).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.MapImagePath, v => v.MapImage.Source,
                    path =>
                    {
                        using Pfim.IImage image = Pfim.Pfim.FromFile(path);
                        return BitmapSource.Create(image.Width, image.Height, 96.0, 96.0, PixelFormat(image),
                            null, image.Data, image.Stride);
                    }).DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GenerateMatchup, v => v.RegenerateButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.GoBack, v => v.BackButton).DisposeWith(d);
            });
        }

        private PixelFormat PixelFormat(Pfim.IImage image)
        {
            switch (image.Format)
            {
                case Pfim.ImageFormat.Rgb24:
                    return PixelFormats.Bgr24;

                case Pfim.ImageFormat.Rgba32:
                    return PixelFormats.Bgr32;

                case Pfim.ImageFormat.Rgb8:
                    return PixelFormats.Gray8;

                case Pfim.ImageFormat.R5g5b5a1:
                case Pfim.ImageFormat.R5g5b5:
                    return PixelFormats.Bgr555;

                case Pfim.ImageFormat.R5g6b5:
                    return PixelFormats.Bgr565;

                default:
                    throw new System.Exception($"Unable to convert {image.Format} to WPF PixelFormat");
            }
        }
    }
}
