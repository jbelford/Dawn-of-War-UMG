﻿using System;
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

                this.Bind(
                        ViewModel,
                        vm => vm.SelectedTab,
                        v => v.GenerationTabControl.SelectedIndex
                    )
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.GenerateMatchupAction, v => v.GenerateButton)
                    .DisposeWith(d);
            });
        }

        private void SetViewModels(GenerationSettingsViewModel vm)
        {
            ModOption.ViewModel = vm.ModViewModel;
            GeneralTab.ViewModel = vm.GeneralTabViewModel;
            GameTab.ViewModel = vm.GameTabViewModel;
            TeamTab.ViewModel = vm.TeamTabViewModel;
        }
    }
}
