using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Null.PhantomTank.Library
{
    public class LockBitmap : IDisposable
    {
        readonly Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public byte[] Pixels { get => pixels; }
        public int Depth { get => depth; }
        public int Width { get => width; }
        public int Height { get => height; }
        public bool IsLocked { get => isLocked; }

        private Func<int, int, Color> colorGetter = (x, y) => throw new InvalidOperationException("Bitmap haven't been locked.");
        private Action<int, int, Color> colorSetter = (x, y, z) => throw new InvalidOperationException("Bitmap haven't been locked.");

        public LockBitmap(Bitmap source)
        {
            this.source = source;
        }

        /// <summary>
        /// Lock bitmap data
        /// </summary>
        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                this.width = source.Width;
                this.height = source.Height;

                // get total locked pixels count
                //int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new System.Drawing.Rectangle(0, 0, Width, Height);

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                // get source bitmap pixel format size
                this.depth = Image.GetPixelFormatSize(source.PixelFormat);

                Iptr = bitmapData.Scan0;
                int stride = bitmapData.Stride;

                unsafe
                {
                    byte* bmpdata = (byte*)Iptr;
                    // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                    if (Depth == 8)
                    {
                        colorGetter = (x, y) =>
                        {
                            int offset = y * stride + x;
                            return Color.FromArgb(bmpdata[offset], bmpdata[offset], bmpdata[offset]);
                        };
                        colorSetter = (x, y, color) =>
                        {
                            int offset = y * stride + x;
                            bmpdata[offset] = color.B;
                        };
                    }
                    else if (Depth == 24)
                    {
                        colorGetter = (x, y) =>
                        {
                            int offset = y * stride + x * 3;
                            return Color.FromArgb(pixels[offset + 2], bmpdata[offset + 1], bmpdata[offset]);
                        };
                        colorSetter = (x, y, color) =>
                        {
                            int offset = y * stride + x * 3;
                            bmpdata[offset] = color.B;
                            bmpdata[offset + 1] = color.G;
                            bmpdata[offset + 2] = color.R;
                        };
                    }
                    else if (Depth == 32)
                    {
                        colorGetter = (x, y) =>
                        {
                            int offset = y * stride + x * 4;
                            return Color.FromArgb(bmpdata[offset + 3], bmpdata[offset + 2], bmpdata[offset + 1], bmpdata[offset]);
                        };
                        colorSetter = (x, y, color) =>
                        {
                            int offset = y * stride + x * 4;
                            bmpdata[offset] = color.B;
                            bmpdata[offset + 1] = color.G;
                            bmpdata[offset + 2] = color.R;
                            bmpdata[offset + 3] = color.A;
                        };
                    }
                    else
                    {
                        throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                    }
                }

                this.isLocked = true;
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
                // Unlock bitmap data
                source.UnlockBits(bitmapData);

                this.isLocked = false;
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
            return colorGetter.Invoke(x, y);
        }

        /// <summary>
        /// Set the color of the specified pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void SetPixel(int x, int y, Color color)
        {
            colorSetter.Invoke(x, y, color);
        }

        public bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < this.width && y > 0 && y < this.height;
        }

        #region IDisposable Support
        private byte[] pixels;
        private int depth;
        private int width;
        private int height;
        private bool isLocked;

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~LockBitmap()
        {
            if (this.isLocked)
                UnlockBits();
        }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            if (this.isLocked)
                UnlockBits();
        }
        #endregion
    }
}
