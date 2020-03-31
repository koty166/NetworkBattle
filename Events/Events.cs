using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ObjectClasses;
using System.Net;

namespace Events
{
    public class EventsClass
    {
        public delegate void PackageSave(object Package, bool IsBullet);

        public delegate void AddToDrawList(Person _p, int _AnimL, ref Image[] Ims, Image Bitmap, int _Pointer = 0);

        public delegate void AddToAddrList(IPAddress Ip , int port);
    }
}
