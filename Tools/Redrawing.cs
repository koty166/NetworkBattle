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
            Bitmap b = new Bitmap(In);
            Color c = b.GetPixel(0, 0);
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    if (b.GetPixel(j, i) == c)
                        b.SetPixel(j, i, Color.FromArgb(0,0,0,255));
                }
            }
            In = b;
            return In;
        }

    }
}
