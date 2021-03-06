using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Null.PhantomTank.Library;

namespace Null.PhantomTank
{
    public static class PhantomTank
    {
        /// 像素运算方式 变量 : 源色: Xc Yc, 输出: Za Zc
        ///   其中: c表示Color, 即颜色亮度, a代表Alpha通道, 即不透明度
        ///   备注: 值全部为0~1的比值
        ///
        /// Expr: | Xc = Za * Zc + 1 - Za
        ///       | Yc = Za * Zc
        /// 
        /// Calc: | Xc = Yc + 1 - Za
        ///       | Yc = Xc + Za - 1
        ///       | Za = Yc - Xc + 1
        /// 
        /// Need: | 1 >= Yc + 1 - Za >= 0, 0 >= Yc - Za >= -1;      Then: | Za >= Yc
        ///       | 1 >= Xc + Za - 1 >= 0, 2 >= Xc + Za >= 1;       Then: | Xc + Za >= 1
        ///       | 1 >= Yc - Xc + 1 >= 0, 0 >= Yc - Xc >= -1;      Then: | Xc >= Yc
        /// 
        /// Root: | Zc = Yc / Za
        ///       | Za = Yc - Xc + 1
        ///       
        ///       Basic Root: | Zc = Yc / (Za / 255) = Yc * 255 / Za
        ///                   | Za = Yc - Xc + 255

        private static Color CalcPixel(Color src1, Color src2, float src1ColorRatio = 0.5f)
        {
            float src2ColorRatio = 1 - src1ColorRatio;

            int
                xc = (int)((src1.R + src1.G + src1.B) * src1ColorRatio / 3 + (src2ColorRatio * 256 - 1)),
                yc = (int)((src2.R + src2.G + src2.B) * src2ColorRatio / 3);

            int
                za = yc - xc + 255,
                zc = za == 0 ? 0 : (yc * 255 / za);

            return Color.FromArgb(za, zc, zc, zc);
        }
        private static float ConvertRatio(float ratio)
        {
            return ratio / (ratio + 1);
        }
        public const float DefaultRatio = 1;
        public static Bitmap ResizeBitmap(Bitmap src, Color bgColor, ResizeMode resize, int newWidth, int newHeight)
        {
            Size srcSize = src.Size;

            int srcWidth = srcSize.Width,
                srcHeight = srcSize.Height;

            Bitmap result = new Bitmap(newWidth, newHeight, src.PixelFormat);
            Graphics rstG = Graphics.FromImage(result);

            rstG.Clear(bgColor);

            Size destSize;
            Rectangle srcRect, destRect;

            switch (resize)
            {
                case ResizeMode.NoResize:
                    destRect = new Rectangle((newWidth - srcWidth) / 2, (newHeight - srcHeight) / 2, srcWidth, srcHeight);
                    rstG.DrawImageUnscaled(src, destRect);
                    break;
                case ResizeMode.Stretch:
                    srcRect = new Rectangle(0, 0, srcWidth, srcHeight);
                    destRect = new Rectangle(0, 0, newWidth, newHeight);
                    rstG.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
                case ResizeMode.Uniform:
                    srcRect = new Rectangle(0, 0, srcWidth, srcHeight);
                    destSize = new Size(newWidth, srcHeight * newWidth / srcWidth);
                    if (destSize.Height > newHeight)
                        destSize = new Size(srcWidth * newHeight / srcHeight, newHeight);
                    destRect = new Rectangle(new Point((newWidth - destSize.Width) / 2, (newHeight - destSize.Height) / 2), destSize);
                    rstG.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
                case ResizeMode.UniformToFill:
                    srcRect = new Rectangle(0, 0, srcWidth, srcHeight);
                    destSize = new Size(newWidth, srcHeight * newWidth / srcWidth);
                    if (destSize.Height < newHeight)
                        destSize = new Size(srcWidth * newHeight / srcHeight, newHeight);
                    destRect = new Rectangle(new Point((newWidth - destSize.Width) / 2, (newHeight - destSize.Height) / 2), destSize);
                    rstG.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 最基本的合成方法, 请保证图片尺寸是一致的
        /// </summary>
        /// <param name="src1">在白底下可以看到的图片</param>
        /// <param name="src2">在黑底下可以看到的图片</param>
        /// <returns>合并后的黑白图像</returns>
        public static Bitmap BasicCombineBitmap(Bitmap src1, Bitmap src2, float colorRatio)
        {
            if (src1 == null || src2 == null)
                throw new ArgumentNullException();
            if (src1.Size != src2.Size)
                throw new ArgumentOutOfRangeException();

            Size srcSize = src1.Size;
            int width = srcSize.Width, height = srcSize.Height;

            Bitmap result = new Bitmap(width, height, src1.PixelFormat);
            result.SetResolution(src1.HorizontalResolution, src1.VerticalResolution);

            LockBitmap lbmp1 = new LockBitmap(src1);
            LockBitmap lbmp2 = new LockBitmap(src2);
            LockBitmap lrst = new LockBitmap(result);
            lbmp1.LockBits();
            lbmp2.LockBits();
            lrst.LockBits();

            float whiteRatio = ConvertRatio(colorRatio);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color
                        srcPixel1 = lbmp1.GetPixel(j, i),
                        srcPixel2 = lbmp2.GetPixel(j, i),
                        outPixel = CalcPixel(srcPixel1, srcPixel2, whiteRatio);

                    lrst.SetPixel(j, i, outPixel);
                }
            }

            lbmp1.UnlockBits();
            lbmp2.UnlockBits();
            lrst.UnlockBits();

            return result;
        }

        /// <summary>
        /// 转换图片
        /// </summary>
        /// <param name="src">源图</param>
        /// <param name="tankType">坦克类型</param>
        /// <returns>转换结果</returns>
        public static Bitmap ConvertBitmap(Bitmap src, TankType tankType)
        {
            Size srcSize = src.Size;

            int
                srcWidth = srcSize.Width,
                srcHeight = srcSize.Height;

            Bitmap result = new Bitmap(srcWidth, srcHeight, src.PixelFormat);
            result.SetResolution(src.HorizontalResolution, src.VerticalResolution);

            LockBitmap lsrc = new LockBitmap(src);
            LockBitmap lrst = new LockBitmap(result);
            lsrc.LockBits();
            lrst.LockBits();

            Func<int, Color> pixelCalcFunc;
            switch (tankType)
            {
                case TankType.AppearOnBlack:
                    pixelCalcFunc = (srcPixel) => Color.FromArgb(srcPixel, 255, 255, 255);
                    break;
                case TankType.AppearOnWhite:
                    pixelCalcFunc = (srcPixel) => Color.FromArgb(255 - srcPixel, 0, 0, 0);
                    break;
                default:
                    throw new InvalidOperationException("Not supported.");
            }

            for (int i = 0; i < srcHeight; i++)
            {
                for (int j = 0; j < srcWidth; j++)
                {
                    Color pixel = lsrc.GetPixel(j, i);
                    int light = (pixel.R + pixel.G + pixel.B) / 3;
                    lrst.SetPixel(j, i, pixelCalcFunc.Invoke(light));
                }
            }

            lsrc.UnlockBits();
            lrst.UnlockBits();

            return result;
        }
        public static Bitmap ConvertImage(Image src, TankType tankType)
        {
            Bitmap newSrc = new Bitmap(src);
            Bitmap result = ConvertBitmap(newSrc, tankType);

            newSrc.Dispose();
            return result;
        }

        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2, Color bgColor1, Color bgColor2, ResizeMode resize, float colorRatio)
        {
            Size
                src1Size = src1.Size,
                src2Size = src2.Size;

            int src1Width = src1Size.Width,
                src1Height = src1Size.Height,
                src2Width = src2Size.Width,
                src2Height = src2Size.Height;

            int
                maxWidth = src1Width > src2Width ? src1Width : src2Width,
                maxHeight = src1Height > src2Height ? src1Height : src2Height;

            Bitmap
                newSrc1 = new Bitmap(maxWidth, maxHeight, src1.PixelFormat),
                newSrc2 = new Bitmap(maxWidth, maxHeight, src2.PixelFormat);

            Graphics
                srcG1 = Graphics.FromImage(newSrc1),
                srcG2 = Graphics.FromImage(newSrc2);

            srcG1.Clear(bgColor1);
            srcG2.Clear(bgColor2);

            // 这里进行的是对图片的重新调整尺寸操作
            switch (resize)
            {
                case ResizeMode.NoResize:
                    srcG1.DrawImageUnscaled(src1, (maxWidth - src1Width) / 2, (maxHeight - src1Height) / 2);
                    srcG2.DrawImageUnscaled(src2, (maxWidth - src2Width) / 2, (maxHeight - src2Height) / 2);
                    break;
                case ResizeMode.Stretch:
                    srcG1.DrawImage(src1, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, src1Width, src1Height), GraphicsUnit.Pixel);
                    srcG2.DrawImage(src2, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, src2Width, src2Height), GraphicsUnit.Pixel);
                    break;
                case ResizeMode.Uniform:
                    Size
                        scaleSize1 = new Size(maxWidth, (int)(src1Height * ((float)maxWidth / src1Width))),
                        scaleSize2 = new Size(maxWidth, (int)(src2Height * ((float)maxWidth / src2Width)));
                    if (scaleSize1.Height > maxHeight)
                        scaleSize1 = new Size((int)(src1Width * ((float)maxHeight / src1Height)), maxHeight);
                    if (scaleSize2.Height > maxHeight)
                        scaleSize2 = new Size((int)(src2Width * ((float)maxHeight / src2Height)), maxHeight);
                    srcG1.DrawImage(src1, new Rectangle(new Point((maxWidth - scaleSize1.Width) / 2, (maxHeight - scaleSize1.Height) / 2), scaleSize1), new Rectangle(0, 0, src1Width, src1Height), GraphicsUnit.Pixel);
                    srcG2.DrawImage(src2, new Rectangle(new Point((maxWidth - scaleSize2.Width) / 2, (maxHeight - scaleSize2.Height) / 2), scaleSize2), new Rectangle(0, 0, src2Width, src2Height), GraphicsUnit.Pixel);
                    break;
                case ResizeMode.UniformToFill:
                    Size
                        scaleFillSize1 = new Size(maxWidth, (int)(src1Height * ((float)maxWidth / src1Width))),
                        scaleFillSize2 = new Size(maxWidth, (int)(src2Height * ((float)maxWidth / src2Width)));
                    if (scaleFillSize1.Height < maxHeight)
                        scaleFillSize1 = new Size((int)(src1Width * ((float)maxHeight / src1Height)), maxHeight);
                    if (scaleFillSize2.Height < maxHeight)
                        scaleFillSize2 = new Size((int)(src2Width * ((float)maxHeight / src2Height)), maxHeight);
                    srcG1.DrawImage(src1, new Rectangle(new Point((maxWidth - scaleFillSize1.Width) / 2, (maxHeight - scaleFillSize1.Height) / 2), scaleFillSize1), new Rectangle(0, 0, src1Width, src1Height), GraphicsUnit.Pixel);
                    srcG2.DrawImage(src2, new Rectangle(new Point((maxWidth - scaleFillSize2.Width) / 2, (maxHeight - scaleFillSize2.Height) / 2), scaleFillSize2), new Rectangle(0, 0, src2Width, src2Height), GraphicsUnit.Pixel);
                    break;
            }

            srcG1.Dispose();
            srcG2.Dispose();

            Bitmap result = BasicCombineBitmap(newSrc1, newSrc2, colorRatio);    // 调用最终合成方法
            newSrc1.Dispose();
            newSrc2.Dispose();

            return result;
        }
        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2, Color bgColor1, Color bgColor2, ResizeMode resize)
        {
            return CombineBitmap(src1, src2, bgColor1, bgColor2, resize, DefaultRatio);
        }
        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2, Color bgColor1, Color bgColor2, float colorRatio)
        {
            return CombineBitmap(src1, src2, bgColor1, bgColor2, ResizeMode.NoResize, colorRatio);
        }
        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2, ResizeMode resize, float colorRatio)
        {
            return CombineBitmap(src1, src2, Color.White, Color.Black, resize, colorRatio);
        }
        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2, ResizeMode resize)
        {
            return CombineBitmap(src1, src2, Color.White, Color.Black, resize, 0);
        }
        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2, float colorRatio)
        {
            return CombineBitmap(src1, src2, Color.White, Color.Black, ResizeMode.NoResize, colorRatio);
        }
        public static Bitmap CombineBitmap(Bitmap src1, Bitmap src2)
        {
            return CombineBitmap(src1, src2, Color.White, Color.Black, ResizeMode.NoResize, DefaultRatio);
        }

        public static Bitmap CombineImage(Image src1, Image src2, Color bgColor1, Color bgColor2, ResizeMode resize, float colorRatio)
        {
            Bitmap
                newSrc1 = new Bitmap(src1),
                newSrc2 = new Bitmap(src2);

            Bitmap result = CombineBitmap(newSrc1, newSrc2, bgColor1, bgColor2, resize, colorRatio);
            newSrc1.Dispose();
            newSrc2.Dispose();

            return result;
        }
        public static Bitmap CombineImage(Image src1, Image src2, Color bgColor1, Color bgColor2, ResizeMode resize)
        {
            return CombineImage(src1, src2, bgColor1, bgColor2, resize, DefaultRatio);
        }
        public static Bitmap CombineImage(Image src1, Image src2, Color bgColor1, Color bgColor2, float colorRatio)
        {
            return CombineImage(src1, src2, bgColor1, bgColor2, ResizeMode.NoResize, colorRatio);
        }
        public static Bitmap CombineImage(Image src1, Image src2, ResizeMode resize, float colorRatio)
        {
            return CombineImage(src1, src2, Color.White, Color.Black, resize, colorRatio);
        }
        public static Bitmap CombineImage(Image src1, Image src2, ResizeMode resize)
        {
            return CombineImage(src1, src2, Color.White, Color.Black, resize, DefaultRatio);
        }
        public static Bitmap CombineImage(Image src1, Image src2, float colorRatio)
        {
            return CombineImage(src1, src2, Color.White, Color.Black, ResizeMode.NoResize, colorRatio);
        }
        public static Bitmap CombineImage(Image src1, Image src2)
        {
            return CombineImage(src1, src2, Color.White, Color.Black, ResizeMode.NoResize, DefaultRatio);
        }
    }
    public enum ResizeMode
    {
        NoResize,
        Stretch,
        Uniform,
        UniformToFill,
    }
    public enum TankType
    {
        AppearOnBlack,
        AppearOnWhite,
    }
}
