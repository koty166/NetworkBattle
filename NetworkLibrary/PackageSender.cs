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
        public void SendPerson(Person Per, float _BulletCurner)
        {
            NetDataPackage Package = ToolsClass.ConvertPersonToNetDataPackege(Per,_BulletCurner);

            Package.BulletCurner = _BulletCurner;

            foreach (var i in AddrIPList)
            {
                SendPersonDataPackage(Package, i);
            }
            FileTools.Log("Person sended to all users");
        }

     

        void SendPersonDataPackage(NetDataPackage Package, IPEndPoint EndPoint)
        {
            byte[] MainPackage;
            UdpClient GetClient = new UdpClient();

            MainPackage =  ToolsClass.ConvertPackageToArray(Package);

            GetClient.Send(MainPackage, MainPackage.Length, EndPoint);
            GetClient.Close();

        }

        public void SendingIniPackage(Person LocalPerson)
        {
            byte[] MainPackage = new byte[26];

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

            int Lenght = Dns.GetHostAddresses(Dns.GetHostName()).Length;
            string s = Dns.GetHostAddresses(Dns.GetHostName())[Lenght - 1].MapToIPv4().ToString();

            MainPackage = ToolsClass.ConvertPackageToArray(ToolsClass.ConvertPersonToNetDataPackege(LocalPerson,10));

            MainPackage[0] = byte.Parse(Strings[0]);
            MainPackage[1] = byte.Parse(Strings[1]);
            MainPackage[2] = byte.Parse(Strings[2]);
            MainPackage[3] = byte.Parse(LocalAddrLast);

            MainPackage[4] = 2;

            for (int i = 1; i < 255; i++)
            {
                Strings[3] = i.ToString();
                Addr = Strings[0] + "." + Strings[1] + "." + Strings[2] + "." + Strings[3];
                if (Addr == s)
                    continue;

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
