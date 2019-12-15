using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DowUmg.Presentation.WPF.Controls
{
    internal class NumericTextBox : TextBox, IActivatableView
    {
        public NumericTextBox()
        {
            this.WhenActivated(d =>
            {
                this.Events().PreviewTextInput
                    .Subscribe(e =>
                    {
                        if (SelectionStart == 0 && Text.Length > 0)
                        {
                            e.Handled = !Regex.IsMatch(e.Text, @"^[1-9][0-9]*$");
                        }
                        else
                        {
                            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
                        }
                    }).DisposeWith(d);
            });
        }
    }
}
