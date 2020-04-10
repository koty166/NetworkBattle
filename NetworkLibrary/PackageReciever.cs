using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ObjectClasses;
using Events;
using System.Threading;
using Tools;

namespace NetworkLibrary
{
    public class PackageReciever : IDisposable
    {
        Thread ConnectWaiter;
        Thread TcpConnectWaiter;
        public PackageReciever()
        { }
        public PackageReciever(EventsClass.PackageSave Ev, List<Person> ls , EventsClass.AddToAddrList AddEv) : base()
        {
            ConnectWaiter = new Thread(new ParameterizedThreadStart(PackageWait));
            TcpConnectWaiter = new Thread(new ParameterizedThreadStart(WaitTcpConnect));
            object[] ob = new object[2];
            ob[0] = Ev;
            ob[1] = ls;

            ConnectWaiter.Start(ob);
            FileTools.Log("Connect waiter start");

            ob = new object[2];
            ob[0] = Ev;
            ob[1] = AddEv;
            TcpConnectWaiter.Start(ob);
            FileTools.Log("TCP Connect waiter start");
        }
              
        static void SendPersonList(object ob)
        {
            IPAddress Addr = IPAddress.Parse((string)((object[])ob)[0]);
            List<Person> ls = (List<Person>)((object[])ob)[1];
            TcpClient Sender = new TcpClient();

            for (int i = 0; i < 128; i++)
            {
                try
                {
                    Sender.Connect(Addr, 7778);
                    break;
                }
                catch { }
            }
            if (!Sender.Connected)
                return;

            Sender.GetStream().Write(BitConverter.GetBytes(ls.Count - 1),0,4);

            byte[] MainPackage = new byte[37], buf;

            string[] Strings = new string[4];
            foreach (var i in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (i.AddressFamily == AddressFamily.InterNetwork)
                    Strings = i.ToString().Split('.');
            }

            FileTools.Log($"Sended ip:{Strings[0]}.{Strings[1]}.{Strings[2]}.{Strings[3]}");

            for (int i = 0; i < ls.Count - 1; i++)
            {
                MainPackage[0] = byte.Parse(Strings[0]);
                MainPackage[1] = byte.Parse(Strings[1]);
                MainPackage[2] = byte.Parse(Strings[2]);
                MainPackage[3] = byte.Parse(Strings[3]);

                MainPackage[4] = 0;

                buf = BitConverter.GetBytes((int)ls[i].ID);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 5, 0, 4);

                buf = BitConverter.GetBytes(ls[i].X);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 9, 0, 4);

                buf = BitConverter.GetBytes(ls[i].Y);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 13, 0, 4);

                buf = BitConverter.GetBytes(ls[i].XSpeed);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 17, 0, 4);

                buf = BitConverter.GetBytes(ls[i].YSpeed);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 21, 0, 4);

                buf = BitConverter.GetBytes(8);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 25, 0, 4);

                buf = BitConverter.GetBytes(8);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 29, 0, 4);

                buf = BitConverter.GetBytes(-1);
                ToolsClass.CopyFromArrayToArry(ref MainPackage, ref buf, 33, 0, 4);

                Sender.GetStream().Write(MainPackage,0,MainPackage.Length);
            }
            FileTools.Log("Person list sended");
            Sender.Close();
        }

        static void PackageWait(object ob)
        {
            EventsClass.PackageSave Event = (EventsClass.PackageSave)((object[])ob)[0];
            List<Person> ls = (List<Person>)((object[])ob)[1];
            while (true)
            {
                PersonNetDataPackage PersonData = new PersonNetDataPackage();
                PackageReciever.GetPackage(PersonData,ls);
                FileTools.Log("Package got");

                Event?.Invoke(PersonData);
            }
        }

        static void ReadFromBuffer(PersonNetDataPackage PersonData, byte[] Buffer, List<Person> ls)
        {
            if (Buffer[4] == 3)
            {
                Thread SendPersons = new Thread(new ParameterizedThreadStart(SendPersonList));
                string RemoveAddr = Buffer[0].ToString() + "." + Buffer[1].ToString() + "." + Buffer[2].ToString() + "." + Buffer[3].ToString();

                object[] ob = new object[2];
                ob[0] = RemoveAddr;
                ob[1] = ls;
                int Lenght = Dns.GetHostAddresses(Dns.GetHostName()).Length;
                string s = Dns.GetHostAddresses(Dns.GetHostName())[Lenght - 1].MapToIPv4().ToString();
                if (RemoveAddr != s)
                {
                    SendPersons.Start(ob);//Start sending person list
                    PersonData.PersonID = -1;
                    PersonData.X = Buffer[0];
                    PersonData.Y = Buffer[1];
                    PersonData.XSpeed = Buffer[2];
                    PersonData.YSpeed = Buffer[3];

                    ls.Add(new Person()
                    {
                        ID = Buffer[4],
                        X = BitConverter.ToInt32(Buffer, 5),
                        Y = BitConverter.ToInt32(Buffer, 9),
                        XSpeed = BitConverter.ToInt32(Buffer, 13),
                        YSpeed = BitConverter.ToInt32(Buffer, 17)
                    });
                }
                else
                {
                    PersonData.PersonID = -2;//this package isn`t avalible addres == local addrers
                }
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
                PersonData.BulletCurner = BitConverter.ToInt32(Buffer, 33);

                if (Buffer[4] == 0)
                    PersonData.IsNewPerson = true;
                else
                    PersonData.IsNewPerson = false;

            }
        }
        
        static void GetPackage(PersonNetDataPackage PersonData, List<Person> ls, int Port = 7777)
        {
            byte[] Buffer;         
            IPEndPoint RemoveIPEndPoint = null;
            UdpClient GetClient = new UdpClient(Port);
            Buffer = GetClient.Receive(ref RemoveIPEndPoint);
            GetClient.Close();
            ReadFromBuffer(PersonData,Buffer,ls);  
        }
        
        static void WaitTcpConnect(object ob)
        {
            EventsClass.PackageSave Event = (EventsClass.PackageSave)((object[])ob)[0];
            EventsClass.AddToAddrList AddEv = (EventsClass.AddToAddrList)((object[])ob)[1];
            TcpListener Listener = new TcpListener(7778);

            Listener.Start();
            TcpClient Client = Listener.AcceptTcpClient();

            byte[] buf = new byte[4];

            Client.GetStream().Read(buf,0,4);
            
            int Length = BitConverter.ToInt32(buf,0);
            for (int i = 0; i < Length; i++)
            {
                buf = new byte[34];
                Client.GetStream().Read(buf,0,34);
                AddEv?.Invoke(IPAddress.Parse(buf[0] + "." + buf[1] + "." + buf[2] + "." + buf[3]), 7777);

                PersonNetDataPackage PersonData = new PersonNetDataPackage();
                ReadFromBuffer(PersonData,buf,null);

                Event?.Invoke(PersonData);
            }
            FileTools.Log("Person list got");
        }

        public void Dispose()
        {       
            UdpClient Close = new UdpClient();
            Close.Send(new byte[] { 0, 5 }, 2,"localhost",7777);
            ConnectWaiter.Abort();
            FileTools.Log("Connect waiter dispose");
        }

        public void DisposeTcpClient()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("localhost", 7778);
                client.Close();
                TcpConnectWaiter.Abort();

                FileTools.Log("TCP Connect waiter dispose");
            }
            catch { FileTools.Log("Error in TCP Connect waiter disposing"); }
        }
    }
}
