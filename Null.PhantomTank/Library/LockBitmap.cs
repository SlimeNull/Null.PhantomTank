using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Null.PhantomTank.Library
{
    public class LockBitmap : IDisposable
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get; set; }
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsLocked { get; private set; }

        private Func<int, Color> colorGetter;
        private Action<int, Color> colorSetter;

        public LockBitmap(Bitmap source)
        {
            this.source = source;
            LockBits();
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                //int PixelCount = Width * Height;

                // Create rectangle to lock
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth == 8)
                {
                    colorGetter = (offset) => Color.FromArgb(Pixels[offset], Pixels[offset], Pixels[offset]);
                    colorSetter = (offset, color) =>
                    {
                        Pixels[offset] = color.B;
                    };
                }
                else if (Depth == 24)
                {
                    colorGetter = (offset) => Color.FromArgb(Pixels[offset + 2], Pixels[offset + 1], Pixels[offset]);
                    colorSetter = (offset, color) =>
                    {
                        Pixels[offset] = color.B;
                        Pixels[offset + 1] = color.G;
                        Pixels[offset + 2] = color.R;
                    };
                }
                else if (Depth == 32)
                {
                    colorGetter = (offset) => Color.FromArgb(Pixels[offset + 3], Pixels[offset + 2], Pixels[offset + 1], Pixels[offset]);
                    colorSetter = (offset, color) =>
                    {
                        Pixels[offset] = color.B;
                        Pixels[offset + 1] = color.G;
                        Pixels[offset + 2] = color.R;
                        Pixels[offset + 3] = color.A;
                    };
                }
                else
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // create byte array to copy pixel values
                int step = Depth / 8;
                Pixels = new byte[bitmapData.Stride * Height];
                Iptr = bitmapData.Scan0;

                IsLocked = true;

                // Copy data from pointer to array
                Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unlock bitmap data
        /// </summary>
        public void UnlockBits()
        {
            try
            {
                // Copy data from byte array to pointer
                Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

                // Unlock bitmap data
                source.UnlockBits(bitmapData);

                IsLocked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;

            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            //int i = ((y * Width) + x) * cCount;
            int i = y * bitmapData.Stride + x * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            // Get color by array index
            clr = colorGetter.Invoke(i);

            return clr;
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            int cCount = Depth / 8;

            // Get start index of the specified pixel
            //int i = ((y * Width) + x) * cCount;
            int i = y * bitmapData.Stride + x * cCount;

            // Set color by array index and color object
            colorSetter.Invoke(i, color);
        }

        public bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < this.Width && y > 0 && y < this.Height;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                UnlockBits();
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~LockBitmap() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
