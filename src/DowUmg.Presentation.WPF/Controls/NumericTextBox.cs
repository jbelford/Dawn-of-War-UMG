using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace DowUmg.Presentation.WPF.Controls
{
    internal class NumericTextBox : TextBox, IActivatableView
    {
        public NumericTextBox()
        {
            this.WhenActivated(d =>
            {
                var reg = new Regex(@"^[0-9]+$");
                this.Events().PreviewTextInput
                    .Subscribe(e =>
                    {
                        e.Handled = !reg.IsMatch(e.Text);
                    }).DisposeWith(d);
            });
        }
    }
}
