using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectClasses;

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
        public static float CountCurner(int X,int Y, float ParentX , float ParentY , out int AnimAddr)
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
            return (float)Curner;
        }
        public static void CountBulletStartPoint(ref int X, ref int Y, double Curner, int Speed = 30)
        { 

            X += (int)(Math.Cos(2 * Math.PI - Curner) * Speed);
            Y += (int)(Math.Sin(2 * Math.PI - Curner) * Speed);
        }

        public static void ActivateAnimation(Person p, int IdleAnimAddr, int MaxAnimAddr, int CurrentAnimAddr)
        {
            p.MaxAnimAddr = MaxAnimAddr;
            p.CurrentAnimAddr = CurrentAnimAddr;
            p.IsIdle = false;
            p.IdleAnimAddr = IdleAnimAddr;
        }

        public static NetDataPackage ConvertPersonToNetDataPackege(Person Per, float _BulletCurner) =>  new NetDataPackage()
        {
                X = Per.X, Y = Per.Y,
                XSpeed = Per.XSpeed, YSpeed = Per.YSpeed,
                ID = Per.ID,
                IsIdle = Per.IsIdle,
                BulletCurner = _BulletCurner,
                CurrentAnimAddr = Per.CurrentAnimAddr,
                IdleAnimAddr = Per.IdleAnimAddr,
                MaxAnimAddr = Per.MaxAnimAddr
            };

        public static NetDataPackage ArrayToNetDataPackege(byte[] Data)
        {
            NetDataPackage PDataOut = new NetDataPackage();

            PDataOut.ID = Data[5];
            PDataOut.X = BitConverter.ToInt32(Data, 5);
            PDataOut.Y = BitConverter.ToInt32(Data, 9);
            PDataOut.XSpeed = BitConverter.ToInt32(Data, 13);
            PDataOut.YSpeed = BitConverter.ToInt32(Data, 17);
            PDataOut.CurrentAnimAddr = BitConverter.ToInt32(Data, 21);
            PDataOut.IdleAnimAddr = BitConverter.ToInt32(Data, 25);
            PDataOut.MaxAnimAddr = BitConverter.ToInt32(Data, 29);
            PDataOut.BulletCurner = BitConverter.ToInt32(Data, 33);

            if (Data[4] == 2)
            {
                PDataOut.ID = 0;
                Array.Copy(Data,0,PDataOut.SourseIp,0,4);

            }

            return PDataOut;
        }

        public static byte[] ConvertPackageToArray(NetDataPackage Package)
        {
            byte[] MainPackage = new byte[37], buf;

            Array.Copy(Package.SourseIp,0,MainPackage,0,4);
                
            MainPackage[3] = Package.ID;

            buf = BitConverter.GetBytes(Package.X);
            CopyFromArrayToArry(ref MainPackage, ref buf, 5, 0, 4);

            buf = BitConverter.GetBytes(Package.Y);
            CopyFromArrayToArry(ref MainPackage, ref buf, 9, 0, 4);

            buf = BitConverter.GetBytes(Package.XSpeed);
            CopyFromArrayToArry(ref MainPackage, ref buf, 13, 0, 4);

            buf = BitConverter.GetBytes(Package.YSpeed);
            CopyFromArrayToArry(ref MainPackage, ref buf, 17, 0, 4);

            buf = BitConverter.GetBytes(Package.CurrentAnimAddr);
            CopyFromArrayToArry(ref MainPackage, ref buf, 21, 0, 4);

            buf = BitConverter.GetBytes(Package.IdleAnimAddr);
            CopyFromArrayToArry(ref MainPackage, ref buf, 25, 0, 4);

            buf = BitConverter.GetBytes(Package.MaxAnimAddr);
            CopyFromArrayToArry(ref MainPackage, ref buf, 29, 0, 4);

            buf = BitConverter.GetBytes(Package.BulletCurner);
            CopyFromArrayToArry(ref MainPackage, ref buf, 33, 0, 4);

            return MainPackage;
        }

    }
}
