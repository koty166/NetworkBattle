using ObjectClasses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Tools;

namespace NetworkLibrary
{
    public class PackageSender
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
            FileTools.Log("Person sended to all users");
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

        void SendPersonDataPackage(PersonNetDataPackage Package, IPEndPoint EndPoint)
        {
            byte[] MainPackage = new byte[34], buf;
            UdpClient GetClient = new UdpClient();

            if (Package.IsNewPerson)
                MainPackage[4] = 0;
            else
                MainPackage[4] = 2;

            buf = BitConverter.GetBytes(Package.PersonID);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 5, 0, 4);

            buf = BitConverter.GetBytes(Package.X);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 9, 0, 4);

            buf = BitConverter.GetBytes(Package.Y);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 13, 0, 4);

            buf = BitConverter.GetBytes(Package.XSpeed);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 17, 0, 4);

            buf = BitConverter.GetBytes(Package.YSpeed);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 21, 0, 4);

            buf = BitConverter.GetBytes(Package.AnimAddr);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 25, 0, 4);

            buf = BitConverter.GetBytes(Package.AnimLenght);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 29, 0, 4);

            GetClient.Send(MainPackage, 34, EndPoint);
            GetClient.Close();

        }

        void SendBulletDataPackage(BulletNetDataPackage Package, IPEndPoint EndPoint)
        {
            byte[] MainPackage = new byte[21], buf;
            UdpClient GetClient = new UdpClient();

            MainPackage[4] = 1;

            buf = BitConverter.GetBytes(Package.Curner);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 5, 0, 4);
            buf = BitConverter.GetBytes(Package.X);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 9, 0, 4);
            buf = BitConverter.GetBytes(Package.Y);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 13, 0, 4);
            buf = BitConverter.GetBytes(Package.ParentPersonID);
            ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 17, 0, 4);


            GetClient.Send(MainPackage, 21, EndPoint);
            GetClient.Close();
        }

        public void SendingIniPackage(Person LocalPerson)
        {
            byte[] MainPackage = new byte[22] , buf;
            MainPackage[4] = 3;

            string[] Strings = new string[4];
            foreach (var i in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (i.AddressFamily == AddressFamily.InterNetwork)
                    Strings = i.ToString().Split('.');
            } 
            FileTools.Log($"Sended ip:{Strings[0]}.{Strings[1]}.{Strings[2]}.{Strings[3]}");
            string LocalAddrLast = Strings[3];

            String Addr;
            UdpClient Client = new UdpClient();

            for (int i = 1; i < 255; i++)
            {
                Strings[3] = i.ToString();
                Addr = Strings[0] + "." + Strings[1] + "." + Strings[2] + "." + Strings[3];
                MainPackage[0] = byte.Parse(Strings[0]);
                MainPackage[1] = byte.Parse(Strings[1]);
                MainPackage[2] = byte.Parse(Strings[2]);
                MainPackage[3] = byte.Parse(LocalAddrLast);

                MainPackage[4] = 3;
                MainPackage[5] = LocalPerson.ID;
                buf = BitConverter.GetBytes(LocalPerson.X);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 6, 0, 4);

                buf = BitConverter.GetBytes(LocalPerson.Y);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 10, 0, 4);

                buf = BitConverter.GetBytes(LocalPerson.XSpeed);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 14, 0, 4);

                buf = BitConverter.GetBytes(LocalPerson.YSpeed);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 18, 0, 4);

                Client.Send(MainPackage, MainPackage.Length, new IPEndPoint(IPAddress.Parse(Addr), 7777));
            }
            FileTools.Log("Ini package sended");
            Client.Close();
        }

        public void AddToAddrIPList(IPAddress Addr, int port = 7777)
        {
            FileTools.Log("Addres added");
            if (!AddrIPList.Contains(new IPEndPoint(Addr, port)))
                AddrIPList.Add(new IPEndPoint(Addr, port));
        }
           
        public void AddToAddrIPList(String Addr, int port = 7777)
        {
            FileTools.Log("Addres added");
            if (!AddrIPList.Contains(new IPEndPoint(IPAddress.Parse(Addr), port)))
                AddrIPList.Add(new IPEndPoint(IPAddress.Parse(Addr), port));
        }

        public IPEndPoint[] GetAddrList() =>
             AddrIPList.ToArray();
    }
            
}
