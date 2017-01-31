using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using DianetMob.Utils;
using Android.Graphics;
using System.IO;
using Android.Util;
using Android.Content.Res;

[assembly: Dependency(typeof(DianetMob.Droid.Utils.ImageResizer_Android))]
namespace DianetMob.Droid.Utils
{
    class ImageResizer_Android: IImageResizer
    {
        public int FdpToPixWidth(float fdp)
        {
            float pixWidth = (float)Resources.System.DisplayMetrics.WidthPixels;
            float fdpWidth = (float)App.Current.MainPage.Width;
            float pixPerDp = pixWidth / fdpWidth;
            return (int)(fdp * pixPerDp);
        }

        public int FdpToPixHeight(float fdp)
        {
            float pixHeight = (float)Resources.System.DisplayMetrics.HeightPixels;
            float fdpHeight = (float)App.Current.MainPage.Height;
            float pixPerDp = pixHeight / fdpHeight;
            return (int)(fdp * pixPerDp);
        }

        public byte[] ResizeImage(byte[] imageData, double width, double height)
        {
            // Load the bitmap 
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            //
            double ZielHoehe = 0;
            double ZielBreite = 0;
            //
            var Hoehe = originalImage.Height;
            var Breite = originalImage.Width;
            //
            if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
            {
                ZielHoehe = height;
                double teiler = Hoehe / height;
                ZielBreite = Breite / teiler;
            }
            else // Breite (61 für Avatar) ist Master
            {
                ZielBreite = width;
                double teiler = Breite / width;
                ZielHoehe = Hoehe / teiler;
            }
            //
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)ZielBreite, (int)ZielHoehe, false);
            // 
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }

    }
}