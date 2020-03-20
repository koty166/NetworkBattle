using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ObjectClasses;
using Events;

namespace NetworkLibrary
{
    internal class PackageReciever
    {

        internal PackageReciever()
        {

        }


        static void PackageWait(object Ev)
        {
            EventsClass.PackageSave Event = (EventsClass.PackageSave)Ev;
            while (true)
            {
                BulletNetDataPackage BulletData = new BulletNetDataPackage();
                PersonNetDataPackage PersonData = new PersonNetDataPackage();
                bool IsBullet = PackageReciever.GetPackage(BulletData, PersonData);

                if (IsBullet)
                    Event?.Invoke(BulletData, true);
                else
                    Event?.Invoke(PersonData, false);
            }
        }
        static bool GetPackage(BulletNetDataPackage BulletData , PersonNetDataPackage PersonData , int Port = 7777)
        {
            byte[] Buffer;         
            IPEndPoint LocalIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),Port);
            UdpClient GetClient = new UdpClient(Port,AddressFamily.InterNetwork);

            Buffer = GetClient.Receive(ref LocalIPEndPoint);
////Сместить на 4 к началу пакета
            if(Buffer[4] == 1)
            {
                BulletData.Curner = BitConverter.ToInt32(Buffer,5);
                BulletData.X = BitConverter.ToInt32(Buffer, 9);
                BulletData.Y = BitConverter.ToInt32(Buffer, 13);
                BulletData.ParentPersonID = BitConverter.ToInt32(Buffer, 17);
                return true;
            }
            else
            {
                PersonData.PersonID = BitConverter.ToInt32(Buffer, 5);
                PersonData.X = BitConverter.ToInt32(Buffer, 9);
                PersonData.Y = BitConverter.ToInt32(Buffer, 13);
                PersonData.XSpeed = BitConverter.ToInt32(Buffer, 17);
                PersonData.YSpeed = BitConverter.ToInt32(Buffer, 21);
                PersonData.AnimAddr = BitConverter.ToInt32(Buffer, 25);
                PersonData.AnimLenght = BitConverter.ToInt32(Buffer, 29);

                if (Buffer[4] == 0)
                    PersonData.IsNewPerson = true;

                return false;
            }
        }
    }
}
