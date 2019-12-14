using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using System;
using System.Text.RegularExpressions;

namespace DowUmg.Presentation.ViewModels
{
    public class NumberInputViewModel : ReactiveObject, IValidatableViewModel
    {
        public NumberInputViewModel(string label, int minValue, int maxValue, int defaultValue = 0)
        {
            Label = label;
            Input = defaultValue.ToString();

            int largest = Math.Max(Math.Abs(minValue), Math.Abs(maxValue));
            Digits = (int)Math.Floor(Math.Log10(largest) + 1) + 1;

            var reg = new Regex(@"^[0-9]+$");
            InputRule = this.ValidationRule(vm => vm.Input,
                input => reg.IsMatch(input),
                $"Must be a number between {minValue} and {maxValue}");
        }

        [Reactive]
        public string Input { get; set; }

        public string Label { get; }

        public int Digits { get; }

        public ValidationContext ValidationContext { get; } = new ValidationContext();

        public ValidationHelper InputRule { get; }
    }
}
