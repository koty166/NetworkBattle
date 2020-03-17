using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ObjectClasses
{
   public class Bullet
    {
        public int Curner;
        const int Size = 5, Speed = 3;
        public double X, Y;

        void CountCoordinats(out double NextX, out double NextY)
        {
            double GetRadian(int Curner) => Curner * Math.PI / 180;
            if (Curner < 90)
            {
                NextX = X + Math.Sin(GetRadian(Curner)) * Speed;
                NextY = Y - Math.Cos(GetRadian(Curner)) * Speed;
            }
            else if (Curner > 90 && Curner < 180)
            {
                NextX = X + Math.Cos(GetRadian(Curner - 90)) * Speed;
                NextY = Y + Math.Sin(GetRadian(Curner - 90)) * Speed;
            }
            else if (Curner > 180 && Curner < 270)
            {
                NextX = X - Math.Sin(GetRadian(Curner - 180)) * Speed;
                NextY = Y + Math.Cos(GetRadian(Curner - 180)) * Speed;
            }
            else if (Curner > 270)
            {
                NextX = X - Math.Cos(GetRadian(Curner - 270)) * Speed;
                NextY = Y - Math.Sin(GetRadian(Curner - 270)) * Speed;
            }
            else if (Curner == 0)
            {
                NextX = X;
                NextY = Y - Speed;
            }
            else if (Curner == 90)
            {
                NextX = X + Speed;
                NextY = Y;
            }
            else if (Curner == 180)
            {
                NextX = X;
                NextY = Y + Speed;
            }
            else if (Curner == 270)
            {
                NextX = X - Speed;
                NextY = Y;
            }
            else
            {
                NextX = 0;
                NextY = 0;
            }
        }

        public bool Tick(Rectangle Border)
        {
            Random r = new Random();
            CountCoordinats(out double NextX, out double NextY);

            if (NextX + Size * 2 > Border.Right || NextY + Size * 2 > Border.Bottom
                || NextX < Border.Left || NextY < Border.Top)
                return false;

            X = NextX;
            Y = NextY;

            return true;

        }
    }
}
