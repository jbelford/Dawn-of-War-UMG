using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DowUmg.Presentation.WPF.Converters
{
    internal class RaceNameToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value ??= "";
            if (value is not string)
            {
                return null;
            }
            return GetBitmapSource(value as string);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotSupportedException();
        }

        public static BitmapSource GetBitmapSource(string name)
        {
            var filename = GetRaceImage(name.ToLower());
            return new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
        }

        private static string GetRaceImage(string race) =>
            race switch
            {
                "13th company" => @"/Resources/13thcompany.png",
                "black templars" => @"/Resources/blackTemplars.png",
                "adeptus mechanicus explorators" => @"/Resources/adeptusMechanicus.png",
                "blood angels" => @"/Resources/bloodAngels.png",
                "chaos" => @"/Resources/chaosMarines.png",
                "chaos daemons" => @"/Resources/chaosDemons.png",
                "chaos marines" => @"/Resources/chaosMarines.png",
                "daemon hunters" => @"/Resources/daemonhunters.png",
                "dark angels" => @"/Resources/darkAngels.png",
                "dark eldar" => @"/Resources/darkEldar.png",
                "death guard" => @"/Resources/deathGuard.png",
                "death korps of krieg" => @"/Resources/deathKorps.png",
                "eldar" => @"/Resources/eldar.png",
                "emperor's children" => @"/Resources/emperorsChildren.png",
                "harlequins" => @"/Resources/harlequins.png",
                "imperial fists siege vanguard" => @"/Resources/imperialFists.png",
                "imperial guard" => @"/Resources/imperialGuard.png",
                "legion of the damned" => @"/Resources/legionOfTheDamned.png",
                "necrons" => @"/Resources/necrons.png",
                "orks" => @"/Resources/orks.png",
                "salamanders" => @"/Resources/salamanders.png",
                "sisters of battle" => @"/Resources/sistersOfBattle.png",
                "space marines" => @"/Resources/spaceMarine.png",
                "steel legion" => @"/Resources/steelLegion.png",
                "tau empire" => @"/Resources/tau.png",
                "thousand sons" => @"/Resources/thousandSons.png",
                "tyranids" => @"/Resources/tyranids.png",
                "witch hunters" => @"/Resources/witchHunters.png",
                "world eaters" => @"/Resources/worldEaters.png",
                "ynarri" => @"/Resources/ynnari.png",
                _ => @"/Resources/genericRace.png"
            };
    }
}
