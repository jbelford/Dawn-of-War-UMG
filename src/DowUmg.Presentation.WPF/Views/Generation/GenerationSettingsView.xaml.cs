using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Views
{
    /// <summary>
    /// Interaction logic for GenerationSettingsView.xaml
    /// </summary>
    public partial class GenerationSettingsView : ReactiveUserControl<GenerationSettingsViewModel>
    {
        public GenerationSettingsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .Where(x => x != null)
                    .Do(SetViewModels)
                    .Subscribe()
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GenerateMatchupAction, v => v.GenerateButton)
                    .DisposeWith(d);
            });
        }

        private void SetViewModels(GenerationSettingsViewModel vm)
        {
            ModOption.Content = vm.Mod;
            GeneralTab.ViewModel = vm.GeneralTab;
            GameTab.ViewModel = vm.GameTab;
            TeamTab.ViewModel = vm.TeamTab;
        }
    }
}
