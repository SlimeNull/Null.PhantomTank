using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Null.PhantomTank.Wpf.Model
{
    public static class ImageConvertHelper
    {
        public static void BeginLoadSource(string imagePath, Action<BitmapSource> callback)
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
        public static void BeginLoadSource(System.Drawing.Image image, Action<BitmapSource> callback)
        {
            new Thread(() =>
            {
                MemoryStream ms = new MemoryStream();
                try
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    image.Dispose();
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
    }
}
