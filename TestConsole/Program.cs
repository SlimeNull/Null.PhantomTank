using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Null.PhantomTank;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Image src1 = Image.FromFile(@"C:\Users\Null\Desktop\點開看老婆.png");
            Image src2 = Image.FromFile(@"C:\Users\Null\Desktop\你怎麽可能有老婆.png");
            Stopwatch watch = new Stopwatch();
            while(true)
            {
                watch.Restart();
                Bitmap rst = PhantomTank.CombineImage(src1, src2, ResizeMode.UniformToFill, 1f / 3);
                watch.Stop();
                rst.Dispose();
                Console.WriteLine($"{watch.ElapsedMilliseconds}ms");
                Console.ReadKey(true);
            }
            //rst.Save(
                //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "temp.jpg"));
        }
    }
}
