using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Converters;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for MissionEditorPage.xaml
    /// </summary>
    public partial class MissionEditorView : ReactiveUserControl<MissionEditorViewModel>
    {
        public MissionEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.SaveButton).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Name, v => v.MissionNameInput.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Description, v => v.MissionDescriptionInput.Text)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Map,
                        v => v.MapImage.Source,
                        map => new MapPathToSourceConverter().CreateSource(map.ImagePath)
                    )
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.Map, v => v.MapName.Text, map => map.Name)
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Map,
                        v => v.MapDescription.Text,
                        map => map.Details
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Map,
                        v => v.MapPlayers.Text,
                        map => $"Players: {map.Players}"
                    )
                    .DisposeWith(d);

                this.OneWayBind(
                        ViewModel,
                        vm => vm.Map,
                        v => v.MapSize.Text,
                        map => $"Size: {map.Size}"
                    )
                    .DisposeWith(d);
            });
        }
    }
}
