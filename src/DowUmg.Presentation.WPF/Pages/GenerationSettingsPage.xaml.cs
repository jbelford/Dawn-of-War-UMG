using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for GenerationSettingsView.xaml
    /// </summary>
    public partial class GenerationSettingsPage : ReactiveUserControl<GenerationViewModel>
    {
        public GenerationSettingsPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .Where(x => x != null)
                    .Do(SetViewModels)
                    .Subscribe()
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GenerateMatchupAction, v => v.GenerateButton).DisposeWith(d);
            });
        }

        private void SetViewModels(GenerationViewModel vm)
        {
            ModOption.Content = vm.Mod;
            GeneralTab.ViewModel = vm.GeneralTab;
            GameTab.ViewModel = vm.GameTab;
            TeamTab.ViewModel = vm.TeamTab;
        }
    }
}
