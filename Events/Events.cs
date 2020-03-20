using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ObjectClasses;

namespace Events
{
    public class EventsClass
    {
        public delegate void PackageSave(object Package, bool IsBullet);
        public event PackageSave PackgeWasGot;

        public delegate void AddToDrawList(Person _p, int _AnimL, ref Image[] Ims, Image Bitmap, int _Pointer = 0);
        public event AddToDrawList EventAddToDrawList;
    }
}
