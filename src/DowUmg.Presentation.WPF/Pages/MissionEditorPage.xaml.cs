using System.Reactive.Disposables;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for MissionEditorPage.xaml
    /// </summary>
    public partial class MissionEditorPage : ReactiveUserControl<MissionEditorViewModel>
    {
        public MissionEditorPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel, vm => vm.CancelCommand, v => v.CancelButton)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Name, v => v.MissionNameInput.Text).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Description, v => v.MissionDescriptionInput.Text)
                    .DisposeWith(d);
            });
        }
    }
}
