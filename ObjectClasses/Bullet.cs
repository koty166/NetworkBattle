using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ObjectClasses
{
   public class Bullet
    {
        public float Curner;
        const int Size = 5, Speed = 5;
        public float X, Y;
        public Person ParentPerson;

        public Bullet()
        { }
        public Bullet(Person _P)
        {
            ParentPerson = _P;
        }

        void CountCoordinats(out float NextX, out float NextY)
        {
             NextX = (int)(Math.Cos(2 * Math.PI - Curner) * Speed) + X;
             NextY = (int)(Math.Sin(2 * Math.PI - Curner) * Speed) + Y;
        }

        public bool Tick(Rectangle Border)
        {
            CountCoordinats(out float NextX, out float NextY);

            if (NextX + Size * 2 > Border.Right || NextY + Size * 2 > Border.Bottom
                || NextX < Border.Left || NextY < Border.Top)
                return false;

            X = NextX;
            Y = NextY;

            return true;

        }
    }
}
