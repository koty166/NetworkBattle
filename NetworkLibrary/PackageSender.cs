using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ObjectClasses;

namespace NetworkLibrary
{
    internal class PackageSender : IPackageSender
    {
        public void SendPersonDataPackage(PersonNetDataPackage Package)
        {
            Console.WriteLine("awd");
        }
        public void SendBulletDataPackage(BulletNetDataPackage Package)
        {

        }
        public void CheckConnection(IPEndPoint Addr)
        {

        }
        public void SendIniPackage(Person Pers)
        {

        }
        public void SendEndPackage()
        {

        }
    }
}
