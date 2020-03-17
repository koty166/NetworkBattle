using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using ObjectClasses;

namespace Network_Battle
{
    sealed internal class BulletDrawTickImage : ObjectForDraw
    {
        SolidBrush Brash;
        Bullet Bul;
        int Width, Height;

        internal BulletDrawTickImage()
        {

        }
        internal BulletDrawTickImage(int _Width, int _Height, Image Bitmap , Bullet _Bul) : base(Bitmap)
        {
            Brash = new SolidBrush(Color.Black);
            Bul = _Bul;
            Width = _Width;
            Height = _Height;
        }
        internal override void Draw(bool IsNeedToDraw)
        {
            if (!Bul.Tick(new Rectangle(0, 0, Width, Height)))
            {
                IsNeedToDestroy = true;
                return;
            }
            g.FillEllipse(Brash, (float)Bul.X, (float)Bul.Y, 5, 5);
            
        }
    }
}
