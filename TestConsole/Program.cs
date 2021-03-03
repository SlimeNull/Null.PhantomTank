using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Null.PhantomTank;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Image src1 = Image.FromFile(@"C: \Users\Null\Desktop\Untitled.png");
            Image src2 = Image.FromFile(@"C:\Users\Null\Desktop\007Zrnsigy1gdqyfurnfpj30ku0fmgoe.jpg");
            Bitmap rst = PhantomTank.CombineImage(src1, src2, ResizeMode.UniformToFill, 1f / 3);
            rst.Save(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "temp.jpg"));
        }
    }
}
