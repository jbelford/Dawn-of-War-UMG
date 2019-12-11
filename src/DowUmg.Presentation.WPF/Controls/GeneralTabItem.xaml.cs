﻿using DowUmg.Presentation.ViewModels;
using ReactiveUI;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Controls;

namespace DowUmg.Presentation.WPF.Controls
{
    /// <summary>
    /// Interaction logic for GeneralTabItem.xaml
    /// </summary>
    public partial class GeneralTabItem : ReactiveUserControl<GeneralTabViewModel>
    {
        public GeneralTabItem()
        {
            InitializeComponent();

            InitComboBox(this.HumanComboBox, 1, 8);
            InitComboBox(this.MinComboBox, 2, 7);
            InitComboBox(this.MaxComboBox, 2, 7);

            this.WhenActivated(d =>
            {
                this.MapSizes.ItemsSource = ViewModel.MapSizes;
                this.MapTypes.ItemsSource = ViewModel.MapTypes;

                this.Bind(ViewModel, vm => vm.HumanPlayers, v => v.HumanComboBox.SelectedIndex,
                    hp => hp - 1, idx => idx + 1).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.MinPlayers, v => v.MinComboBox.SelectedIndex,
                    mp => mp - 2, idx => idx + 2).DisposeWith(d);

                this.Bind(ViewModel, vm => vm.MaxPlayers, v => v.MaxComboBox.SelectedIndex,
                    mp => mp - 2, idx => idx + 2).DisposeWith(d);
            });
        }

        private void InitComboBox(ComboBox box, int rangeStart, int count)
        {
            box.Items.Clear();
            foreach (var x in Enumerable.Range(rangeStart, count))
            {
                box.Items.Add(x);
            }
        }
    }
}
