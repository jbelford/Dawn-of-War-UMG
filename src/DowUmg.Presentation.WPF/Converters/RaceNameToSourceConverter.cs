using System;
using System.Globalization;
using System.IO;
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
            var filename = GetRaceImage(Path.GetFileNameWithoutExtension(name).ToLower());
            return new BitmapImage(new Uri(filename, UriKind.RelativeOrAbsolute));
        }

        private static string GetRaceImage(string race) =>
            race switch
            {
                "chaos_marine_race" => @"/Resources/chaosMarines.png",
                "dark_eldar_race" => @"/Resources/darkEldar.png",
                "darkangels_race" => @"/Resources/darkAngels.png",
                "deamonkin_race" => @"/Resources/worldEaters.png",
                "deamons_race" => @"/Resources/chaosDemons.png",
                "death_angels_race" => @"/Resources/darkAngels.png",
                "death_guard_race" => @"/Resources/deathGuard.png",
                "eldar_race" => @"/Resources/eldar.png",
                "emperors_children_race" => @"/Resources/emperorsChildren.png",
                "enclaves_race" => @"/Resources/enclaves.png",
                "fallen_angels_race" => @"/Resources/fallenAngels.png",
                "firstborn_race" => @"/Resources/firstborn.png",
                "guard_race" => @"/Resources/imperialGuard.png",
                "harlequin_race" => @"/Resources/harlequins.png",
                "imperial_fists_race" => @"/Resources/imperialFists.png",
                "inquisition_daemonhunt_race" => @"/Resources/daemonhunters.png",
                "khorne_marine_race" => @"/Resources/worldEaters.png",
                "krieg_race" => @"/Resources/deathKorps.png",
                "lotd_race" => @"/Resources/legionOfTheDamned.png",
                "mech_guard_race" => @"/Resources/adeptusMechanicus.png",
                "mechanicus_race" => @"/Resources/explorators.png",
                "necron_race" => @"/Resources/necrons.png",
                "night_lords_race" => @"/Resources/nightLords.png",
                "ork_race" => @"/Resources/orks.png",
                "praetorian_race" => @"/Resources/praetorianGuard.png",
                "raven_guard_race" => @"/Resources/ravenGuard.png",
                "renegade_guard_race" => @"/Resources/renegade.png",
                "salamanders_race" => @"/Resources/salamanders.png",
                "sisters_race" => @"/Resources/sistersOfBattle.png",
                "space_angels_race" => @"/Resources/bloodAngels.png",
                "space_knight_race" => @"/Resources/greyKnights.png",
                "space_marine_race" => @"/Resources/spaceMarine.png",
                "space_wolves_race" => @"/Resources/spaceWolves.png",
                "ss_blood_angels_race" => @"/Resources/bloodAngels.png",
                "steel_legion_race" => @"/Resources/steelLegion.png",
                "tau_race" => @"/Resources/tau.png",
                "templar_race" => @"/Resources/blackTemplars.png",
                "thirteenth_company_race" => @"/Resources/13thcompany.png",
                "thousand_sons_race" => @"/Resources/thousandSons.png",
                "tyranids_race" => @"/Resources/tyranids.png",
                "warp_daemons_race" => @"/Resources/chaosDemons.png",
                "witch_hunters_race" => @"/Resources/witchHunters.png",
                "ynnari_race" => @"/Resources/ynnari.png",
                _ => @"/Resources/genericRace.png"
            };
    }
}
