using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Null.PhantomTank.Wpf.Model
{
    public static class ImageConvertHelper
    {
        public static void BeginLoadSource(string imagePath, Action<BitmapImage> callback)
        {
            new Thread(() =>
            {
                MemoryStream ms = new MemoryStream();
                try
                {
                    System.Drawing.Image src = System.Drawing.Image.FromFile(imagePath);
                    src.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    src.Dispose();
                    BitmapImage result = new BitmapImage();
                    result.BeginInit();
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.StreamSource = ms;
                    result.EndInit();
                    result.Freeze();
                    callback.Invoke(result);
                }
                catch
                {
                    callback.Invoke(null);
                }
                finally
                {
                    ms.Dispose();
                }
            }).Start();
        }
        public static void BeginLoadSource(System.Drawing.Image image, Action<BitmapImage> callback)
        {
            new Thread(() =>
            {
                MemoryStream ms = new MemoryStream();
                try
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    BitmapImage result = new BitmapImage();
                    result.BeginInit();
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.StreamSource = ms;
                    result.EndInit();
                    result.Freeze();
                    callback.Invoke(result);
                }
                catch
                {
                    callback.Invoke(null);
                }
                finally
                {
                    ms.Dispose();
                }
            }).Start();
        }
        public static void BeginLoadImage(string imagePath, Action<System.Drawing.Image> callback)
        {
            new Thread(() =>
            {
                try
                {
                    System.Drawing.Image src = System.Drawing.Image.FromFile(imagePath);
                    callback.Invoke(src);
                }
                catch
                {
                    callback.Invoke(null);
                }
            }).Start();
        }
        public static void BeginCombineImage(System.Drawing.Image src1, System.Drawing.Image src2, ResizeMode resize, float ratio, Action<System.Drawing.Bitmap> callback)
        {
            new Thread(() =>
            {
                try
                {
                    System.Drawing.Bitmap result = PhantomTank.CombineImage(src1, src2, resize, ratio);
                    callback.Invoke(result);
                }
                catch
                {
                    callback.Invoke(null);
                }
            }).Start();
        }
        public static void BeginConvertImage(System.Drawing.Image src, TankType type, Action<System.Drawing.Bitmap> callback)
        {
            new Thread(() =>
            {
                try
                {
                    System.Drawing.Bitmap result = PhantomTank.ConvertImage(src, type);
                    callback.Invoke(result);
                }
                catch
                {
                    callback.Invoke(null);
                }
            }).Start();
        }
    }
}
