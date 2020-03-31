using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using ObjectClasses;

namespace Network_Battle
{
    internal class ObjectForDraw
    {
        internal Graphics g;
        internal bool IsNeedToDestroy = false;

        internal ObjectForDraw()
        { 
            g = null;
        }
        internal ObjectForDraw(Image Bitmap) : this()
        {
            while(true)
            try
            {
                lock (Bitmap)
                    g = Graphics.FromImage(Bitmap);
                    break;
            }
            catch { }
        }

        virtual internal void Draw(bool i)
        {
        }

    }
}
