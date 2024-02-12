using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ReactiveUI;

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
            return CreateSource(value as string);
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
            result = CreateSource(from as string);
            return true;
        }

        public BitmapSource CreateSource(string path)
        {
            try
            {
                using Pfim.IImage image = Pfim.Pfimage.FromFile(path);
                return BitmapSource.Create(
                    image.Width,
                    image.Height,
                    96.0,
                    96.0,
                    PixelFormat(image),
                    null,
                    image.Data,
                    image.Stride
                );
            }
            catch (Exception)
            {
                return ConvertBitmapToSource(Properties.Resources.missingImage);
            }
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
                    throw new System.Exception(
                        $"Unable to convert {image.Format} to WPF PixelFormat"
                    );
            }
        }

        private BitmapSource ConvertBitmapToSource(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
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
}
