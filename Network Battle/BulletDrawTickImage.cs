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
        internal Bullet Bul;
        int ControlWidth, ControlHeight;
        internal BulletDrawTickImage()
        { }
        internal BulletDrawTickImage(int _ControlWidth, int _ControlHeight, Image Bitmap , Bullet _Bul) : base(Bitmap)
        {
            Brash = new SolidBrush(Color.Black);
            Bul = _Bul;
            ControlWidth = _ControlWidth;
            ControlHeight = _ControlHeight;
        }
        internal override void Draw(bool IsNeedToDraw)
        {
            if (!Bul.Tick(new Rectangle(0, 0, ControlWidth, ControlHeight)))
            {
                IsNeedToDestroy = true;
                return;
            }
            g.FillEllipse(Brash, (float)Bul.X, (float)Bul.Y, 5, 5);
            
        }

    }
}
