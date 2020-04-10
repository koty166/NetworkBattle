using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class ToolsClass
    {
        public static void CopyFromArrayToArry(ref byte[] FirstArray, ref byte[] SecoundArray, int PointerA, int PointerB, int Lenght)
        {
            for (int i = PointerB; i < Lenght + PointerB; i++)
            {
                FirstArray[PointerA] = SecoundArray[i];
                PointerA++;
            }
        }
        public static double CountCurner(int X,int Y, int ParentX , int ParentY , out int AnimAddr)
        {
            double Curner = 0; 
            double OX = ParentX + 48, OY = ParentY + 48;

            if (X > OX && Y < OY)
            {
                AnimAddr = 7;
                Curner = Math.Atan2(OY - Y, X - OX);
            }
            else if (X < OX && Y < OY)
            {
                AnimAddr = 5;
                Curner = Math.PI -  Math.Atan2(OY - Y, OX - X);
            }
            else if (X < OX && Y > OY)
            {
                AnimAddr = 3;
                Curner = (Math.PI - Math.Atan2(Y - ParentY, OX - X)) * -1;
            }
            else if (X > OY && Y > OY)
            {
                AnimAddr = 2;
                Curner = Math.Atan2(Y - OY,X - OX) * -1;
            }
            else if (X < OX && Y == OY)
            {
                AnimAddr = 4;
                Curner = Math.PI;
            }
            else if (X > OX && Y == OY)
            {
                AnimAddr = 0;
                Curner = 0;
            }
            else if (X == OX && Y > OY)
            {
                AnimAddr = 2;
                Curner = Math.PI *3 / 2;
            }
            else if (X == OX && Y < OY)
            {
                AnimAddr = 6;
                Curner = Math.PI / 2;
            }

            else AnimAddr = -1;
            return Curner;
        }
        public static void CountBulletStartPoint(ref int X, ref int Y, double Curner, int Speed = 30)
        { 

            X += (int)(Math.Cos(2 * Math.PI - Curner) * Speed);
            Y += (int)(Math.Sin(2 * Math.PI - Curner) * Speed);
        }

    }
}
