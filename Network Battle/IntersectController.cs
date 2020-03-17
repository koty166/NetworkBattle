using System;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using ObjectClasses;

namespace Network_Battle
{
   internal class IntersectController : IDisposable
    {
        ObjectDrawer ObjDrawer;
        List<Person> PersList;
        List<Bullet> BulletList;
        Thread IntersectThread;
        internal IntersectController()
        { }

        internal IntersectController(ObjectDrawer Ob)
        {
            ObjDrawer = Ob;
            PersList = new List<Person>();
            BulletList = new List<Bullet>();
            IntersectThread = new Thread(new ParameterizedThreadStart(CountCollision));

            IntersectThread.Start(this);
        }

        static void Initialize(IntersectController In)
        {
            In.PersList.Clear();
            In.BulletList.Clear();

            lock(In.ObjDrawer.ObjectTicks)
                foreach (var i in In.ObjDrawer.ObjectTicks)
                {
                    if(i.GetType() == new PersonDrawTickImage().GetType() && ((PersonDrawTickImage)i).IsNeedToDestroy == false)
                        In.PersList.Add(((PersonDrawTickImage)i)._Person);
                    else if(i.GetType() == new BulletDrawTickImage().GetType() && ((BulletDrawTickImage)i).IsNeedToDestroy == false)
                        In.BulletList.Add(((BulletDrawTickImage)i).Bul);
                }
        }
        static void CountCollision(object In)
        {
            IntersectController IC = (IntersectController)In;
            while(true)
            {
                Initialize(IC);
                foreach (var i in IC.PersList)
                {
                    Rectangle R1 = new Rectangle(i.X,i.Y,60,70);
                    foreach (var j in IC.BulletList)
                    {
                        if (j.ParentPerson == i) continue;
                        Rectangle R2 = new Rectangle((int)j.X,(int)j.Y,5,5);
                        if (R1.IntersectsWith(R2))
                            IC.ObjDrawer.IsPersonInList(i,true);
                    }
                }

            }
            
        }
        public void Dispose()
        {
            IntersectThread.Abort();
        }
    }
}
