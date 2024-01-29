﻿using Newtonsoft.Json.Linq;
using ReactiveUI;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DowUmg.Presentation.WPF.Converters
{
    internal class MapPathToSourceConverter : IValueConverter, IBindingTypeConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                return null;
            }
            return createSource(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            return 2;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (!(from is string))
            {
                result = null;
                return false;
            }
            result = createSource(from as string);
            return true;
        }

        private BitmapSource createSource(string path)
        {
            using Pfim.IImage image = Pfim.Pfim.FromFile(path);
            return BitmapSource.Create(image.Width, image.Height, 96.0, 96.0, PixelFormat(image),
                null, image.Data, image.Stride);
        }

        private PixelFormat PixelFormat(Pfim.IImage image)
        {
            switch (image.Format)
            {
                case Pfim.ImageFormat.Rgb24:
                    return PixelFormats.Bgr24;

                case Pfim.ImageFormat.Rgba32:
                    return PixelFormats.Bgr32;

                case Pfim.ImageFormat.Rgb8:
                    return PixelFormats.Gray8;

                case Pfim.ImageFormat.R5g5b5a1:
                case Pfim.ImageFormat.R5g5b5:
                    return PixelFormats.Bgr555;

                case Pfim.ImageFormat.R5g6b5:
                    return PixelFormats.Bgr565;

                default:
                    throw new System.Exception($"Unable to convert {image.Format} to WPF PixelFormat");
            }
        }
    }
}
