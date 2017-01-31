using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DianetMob.Utils;
using UIKit;
using System.Drawing;
using Xamarin.Forms;

[assembly: Dependency(typeof(DianetMob.iOS.Utils.ImageResizer_IOS))]
namespace DianetMob.iOS.Utils
{
    public class ImageResizer_IOS:IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, double width, double height)
        {
            // Load the bitmap
            UIImage originalImage = ImageFromByteArray(imageData);
            //
            var Hoehe = originalImage.Size.Height;
            var Breite = originalImage.Size.Width;
            //
            double ZielHoehe = 0;
            double ZielBreite = 0;
            //

            if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
            {
                ZielHoehe = height;
                double teiler = Hoehe / height;
                ZielBreite = Breite / teiler;
            }
            else // Breite (61 for Avatar) ist Master
            {
                ZielBreite = width;
                double teiler = Breite / width;
                ZielHoehe = Hoehe / teiler;
            }
            //
            width = (double)ZielBreite;
            height = (double)ZielHoehe;
            //
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            originalImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            //
            var bytesImagen = resizedImage.AsJPEG().ToArray();
            resizedImage.Dispose();
            return bytesImagen;
        }
        //
        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            //
            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}