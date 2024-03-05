using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DowUmg.Presentation.WPF.Converters
{
    internal class RaceBitMap(Bitmap bitmap)
    {
        private static readonly RaceBitMap chaosMarines = new(Properties.Resources.chaosMarines);
        private static readonly Dictionary<string, RaceBitMap> raceToImageMap =
            new()
            {
                { "13th company", new(Properties.Resources._13thcompany) },
                { "black templars", new(Properties.Resources.blackTemplars) },
                { "adeptus mechanicus explorators", new(Properties.Resources.adeptusMechanicus) },
                { "blood angels", new(Properties.Resources.bloodAngels) },
                { "chaos", chaosMarines },
                { "chaos daemons", new(Properties.Resources.chaosDemons) },
                { "chaos marines", chaosMarines },
                { "daemon hunters", new(Properties.Resources.daemonhunters) },
                { "dark angels", new(Properties.Resources.darkAngels) },
                { "dark eldar", new(Properties.Resources.darkEldar) },
                { "death guard", new(Properties.Resources.deathGuard) },
                { "death korps of krieg", new(Properties.Resources.deathKorps) },
                { "eldar", new(Properties.Resources.eldar) },
                { "emperor's children", new(Properties.Resources.emperorsChildren) },
                { "harlequins", new(Properties.Resources.harlequins) },
                { "imperial fists siege vanguard", new(Properties.Resources.imperialFists) },
                { "imperial guard", new(Properties.Resources.imperialGuard) },
                { "legion of the damned", new(Properties.Resources.legionOfTheDamned) },
                { "necrons", new(Properties.Resources.necrons) },
                { "orks", new(Properties.Resources.orks) },
                { "salamanders", new(Properties.Resources.salamanders) },
                { "sisters of battle", new(Properties.Resources.sistersOfBattle) },
                { "space marines", new(Properties.Resources.spaceMarine) },
                { "steel legion", new(Properties.Resources.steelLegion) },
                { "tau empire", new(Properties.Resources.tau) },
                { "thousand sons", new(Properties.Resources.thousandSons) },
                { "tyranids", new(Properties.Resources.tyranids) },
                { "witch hunters", new(Properties.Resources.witchHunters) },
                { "world eaters", new(Properties.Resources.worldEaters) },
                { "ynarri", new(Properties.Resources.ynnari) },
            };

        private static readonly BitmapSource defaultImage = ConvertBitmapToSource(
            Properties.Resources.genericRace
        );

        private readonly Bitmap _bitmap = bitmap;
        private BitmapSource _source;

        public BitmapSource Source
        {
            get
            {
                _source ??= ConvertBitmapToSource(_bitmap);
                return _source;
            }
        }

        public static BitmapSource GetBitmapSource(string name)
        {
            raceToImageMap.TryGetValue(name.ToLower().Trim(), out RaceBitMap bitmapSource);
            return bitmapSource?.Source ?? defaultImage;
        }

        internal static BitmapSource ConvertBitmapToSource(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                bitmap.PixelFormat
            );

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width,
                bitmapData.Height,
                bitmap.HorizontalResolution,
                bitmap.VerticalResolution,
                PixelFormats.Bgr32,
                null,
                bitmapData.Scan0,
                bitmapData.Stride * bitmapData.Height,
                bitmapData.Stride
            );

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
    }

    internal class RaceNameToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value ??= "";
            if (value is not string)
            {
                return null;
            }
            return RaceBitMap.GetBitmapSource(value as string);
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
    }
}
