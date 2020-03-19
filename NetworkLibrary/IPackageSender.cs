using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ObjectClasses;

namespace NetworkLibrary
{
    internal interface IPackageSender
    {
        void SendPersonDataPackage(PersonNetDataPackage Package);
        void SendBulletDataPackage(BulletNetDataPackage Package);
        void CheckConnection(IPEndPoint Addr);
        void SendIniPackage(Person Pers);
        void SendEndPackage();
        //В принципе инрефейс тут нахер не нужен
        //но для отработки наследования и ООП я сюда его вставил 
    }
}
