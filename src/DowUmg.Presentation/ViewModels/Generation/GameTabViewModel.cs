using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DowUmg.Constants;
using DynamicData;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    internal record EnumModel<T>(T Model, NumberInputViewModel NumberInput);

    public class GameTabViewModel : ReactiveObject
    {
        public GameTabViewModel()
        {
            DiffOption = Enum.GetValues(typeof(GameDifficulty))
                .Cast<GameDifficulty>()
                .Select(diff => new EnumModel<GameDifficulty>(
                    diff,
                    new NumberInputViewModel(diff.GetName(), 100)
                ))
                .ToList();

            SpeedOption = Enum.GetValues(typeof(GameSpeed))
                .Cast<GameSpeed>()
                .Select(speed => new EnumModel<GameSpeed>(
                    speed,
                    new NumberInputViewModel(speed.GetName(), 100)
                ))
                .ToList();

            RateOption = Enum.GetValues(typeof(GameResourceRate))
                .Cast<GameResourceRate>()
                .Select(rate => new EnumModel<GameResourceRate>(
                    rate,
                    new NumberInputViewModel(rate.GetName(), 100)
                ))
                .ToList();

            StartingOption = Enum.GetValues(typeof(GameStartResource))
                .Cast<GameStartResource>()
                .Select(starting => new EnumModel<GameStartResource>(
                    starting,
                    new NumberInputViewModel(starting.GetName(), 100)
                ))
                .ToList();

            DiffOptionViewModel = new ProportionalOptionsViewModel(
                "Difficulty",
                DiffOption.Select(item => item.NumberInput)
            );

            SpeedOptionViewModel = new ProportionalOptionsViewModel(
                "Game Speed",
                SpeedOption.Select(item => item.NumberInput)
            );
            RateOptionViewModel = new ProportionalOptionsViewModel(
                "Resource Rate",
                RateOption.Select(item => item.NumberInput)
            );
            StartingOptionViewModel = new ProportionalOptionsViewModel(
                "Starting Resources",
                StartingOption.Select(item => item.NumberInput)
            );
        }

        internal IList<EnumModel<GameDifficulty>> DiffOption { get; }
        public ProportionalOptionsViewModel DiffOptionViewModel { get; set; }

        internal IList<EnumModel<GameSpeed>> SpeedOption { get; }
        public ProportionalOptionsViewModel SpeedOptionViewModel { get; set; }

        internal IList<EnumModel<GameResourceRate>> RateOption { get; }
        public ProportionalOptionsViewModel RateOptionViewModel { get; set; }

        internal IList<EnumModel<GameStartResource>> StartingOption { get; }
        public ProportionalOptionsViewModel StartingOptionViewModel { get; set; }
    }
}
