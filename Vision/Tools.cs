using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;

namespace iSpyApplication.Vision
{
    public static class Tools
    {
        public static int BytesPerPixel(PixelFormat pixelFormat)
        {
            int bytesPerPixel;

            // calculate bytes per pixel
            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    return 1;
                case PixelFormat.Format16bppGrayScale:
                    return 2;
                case PixelFormat.Format24bppRgb:
                    return 3;
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    return 4;
                case PixelFormat.Format48bppRgb:
                    return 6;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 8;
                default:
                    throw new UnsupportedImageFormatException("Can not create image with specified pixel format.");
            }
        }

        public static void ConvertToGrayscale(UnmanagedImage source, UnmanagedImage destination)
        {
            if (source.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                Grayscale.CommonAlgorithms.BT709.Apply(source, destination);
            }
            else
            {
                source.Copy(destination);
            }
        }
    }
}
