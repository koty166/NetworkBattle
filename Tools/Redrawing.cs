using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools
{
    public static class Redrawing
    {
       public static Image Redraw(Image In)
        {
            Stopwatch s1 = Stopwatch.StartNew();
            Bitmap b = new Bitmap(In);
            Color c = b.GetPixel(0, 0);
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    if (b.GetPixel(j, i) == c)
                        b.SetPixel(j, i, Color.Transparent);
                }
            }
            In = b;
            Console.WriteLine(s1.ElapsedMilliseconds.ToString());
            return In;
        }

    }
}
