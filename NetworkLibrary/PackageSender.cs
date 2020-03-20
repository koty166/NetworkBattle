using ObjectClasses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace NetworkLibrary
{
    internal class PackageSender
    {
        List<IPEndPoint> AddrIPList;
        public PackageSender()
        {
            AddrIPList = new List<IPEndPoint>();
        }
        public void SendPerson(Person Per, int AnimAddr, int AnimLenght, bool IsNewPerson)
        {
            PersonNetDataPackage Package = new PersonNetDataPackage()
            {
                PersonID = Per.ID,
                X = Per.X,
                Y = Per.Y,
                XSpeed = Per.XSpeed,
                YSpeed = Per.YSpeed,
                AnimAddr = AnimAddr,
                AnimLenght = AnimLenght,
                IsNewPerson = IsNewPerson
            };

            foreach (var i in AddrIPList)
            {
                SendPersonDataPackage(Package, i);
            }
        }
        public void SendBullet(Bullet Bul)
        {
            BulletNetDataPackage Package = new BulletNetDataPackage()
            {
                Curner = Bul.Curner,
                X = (int)Bul.X,
                Y = (int)Bul.Y,
                ParentPersonID = Bul.ParentPerson.ID
            };

            foreach (var i in AddrIPList)
            {
                SendBulletDataPackage(Package, i);
            }
        }

        void CopyFromArrayToArry(ref byte[] FirstArray, ref byte[] SecoundArray, int PointerA, int PointerB, int Lenght)
        {
            int A = PointerA;
            for (int i = PointerB; i < Lenght + PointerB; i++)
            {
                FirstArray[A] = SecoundArray[i];
                A++;
            }
        }

        void SendPersonDataPackage(PersonNetDataPackage Package, IPEndPoint EndPoint)
        {
            byte[] MainPackage = new byte[34], buf;
            UdpClient GetClient = new UdpClient(EndPoint);

            if (Package.IsNewPerson)
                MainPackage[4] = 0;
            else
                MainPackage[4] = 2;

            buf = BitConverter.GetBytes(Package.PersonID);
            CopyFromArrayToArry(ref MainPackage, ref buf, 5, 0, 4);

            buf = BitConverter.GetBytes(Package.X);
            CopyFromArrayToArry(ref MainPackage, ref buf, 9, 0, 4);

            buf = BitConverter.GetBytes(Package.Y);
            CopyFromArrayToArry(ref MainPackage, ref buf, 13, 0, 4);

            buf = BitConverter.GetBytes(Package.XSpeed);
            CopyFromArrayToArry(ref MainPackage, ref buf, 17, 0, 4);

            buf = BitConverter.GetBytes(Package.YSpeed);
            CopyFromArrayToArry(ref MainPackage, ref buf, 21, 0, 4);

            buf = BitConverter.GetBytes(Package.AnimAddr);
            CopyFromArrayToArry(ref MainPackage, ref buf, 25, 0, 4);

            buf = BitConverter.GetBytes(Package.AnimLenght);
            CopyFromArrayToArry(ref MainPackage, ref buf, 29, 0, 4);

            GetClient.Send(MainPackage, 34, EndPoint);

        }

        void SendBulletDataPackage(BulletNetDataPackage Package, IPEndPoint EndPoint)
        {
            byte[] MainPackage = new byte[21], buf;
            UdpClient GetClient = new UdpClient(EndPoint);

            MainPackage[4] = 1;

            buf = BitConverter.GetBytes(Package.Curner);
            CopyFromArrayToArry(ref MainPackage, ref buf, 5, 0, 4);
            buf = BitConverter.GetBytes(Package.X);
            CopyFromArrayToArry(ref MainPackage, ref buf, 9, 0, 4);
            buf = BitConverter.GetBytes(Package.Y);
            CopyFromArrayToArry(ref MainPackage, ref buf, 13, 0, 4);
            buf = BitConverter.GetBytes(Package.ParentPersonID);
            CopyFromArrayToArry(ref MainPackage, ref buf, 17, 0, 4);

            GetClient.Send(MainPackage, 21, EndPoint);
        }

        void CheckConnection(IPEndPoint Addr)
        {

        }

        public void AddToAddrIPList(IPAddress Addr, int port = 7777) =>
            AddrIPList.Add(new IPEndPoint(Addr, port));
        public void AddToAddrIPList(String Addr, int port = 7777) =>
            AddrIPList.Add(new IPEndPoint(IPAddress.Parse(Addr), port));
    }
}
