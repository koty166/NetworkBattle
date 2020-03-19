using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using ObjectClasses;
using System.Diagnostics;

namespace Network_Battle
{
   public class ObjectDrawer : IDisposable
    {
        internal List<ObjectForDraw> ObjectTicks;
        Thread ThObjectDrawer;

        public ObjectDrawer(Image Bitmap,MainWindow.AddToDrawList ev)
        {
            ObjectTicks = new List<ObjectForDraw>();

            ThObjectDrawer = new Thread(new ParameterizedThreadStart(Draw));

            object[] m = new object[3];
            m[0] = Bitmap;
            m[1] = ObjectTicks;
            m[2] = ev;

            ThObjectDrawer.Start(m);
        }

        static void Draw(object m)
        {
            Graphics g;
            lock ((Image)((object[])m)[0])
                g = Graphics.FromImage((Image)((object[])m)[0]);

            List<ObjectForDraw> ListObj = (List<ObjectForDraw>)((object[])m)[1];

            MainWindow.AddToDrawList Event = ((MainWindow.AddToDrawList)((object[])m)[2]);

            int Pause = 25;
            const int MaxTicks = 4;
            int Ticks = 1;
            bool IsNeedToDraw = true;

            while (!false)
            {
                g.Clear(Color.AntiqueWhite);
                lock (ListObj)
                {
                    foreach (var i in ListObj)
                        i.Draw(IsNeedToDraw);

                    for (int i = 0; i < ListObj.Count; i++)
                        if (ListObj[i].IsNeedToDestroy)
                        {
                            if (ListObj[i].GetType() == new PersonDrawTickImage().GetType())
                                ((PersonDrawTickImage)ListObj[i]).InvokeEventForAddToDrawList(Event);
                            ListObj.RemoveAt(i);
                        } 
                }
                Thread.Sleep(Pause);
                
                if (Ticks == 1)
                    IsNeedToDraw = false;
                else if(Ticks == MaxTicks)
                {
                    Ticks = 0;
                    IsNeedToDraw = true;
                }
                Ticks++;
            }
        }

        public bool IsPersonInList(Person _P , bool IsNeedToRemove)
        {
            lock (ObjectTicks)
            {
                for (int i = 0; i < ObjectTicks.Count; i++)
                {
                    if (ObjectTicks[i].GetType() == new PersonDrawTickImage().GetType() && ((PersonDrawTickImage)ObjectTicks[i]).IsPersonEquals(_P))
                    {
                        if (IsNeedToRemove)
                            ObjectTicks.RemoveAt(i);
                        return true;

                    }
                        
                }
            }
            return false;
        }
        public void Dispose()
        {
            ThObjectDrawer.Abort();
        }
        public void AddToObjectTicksList(Person _p, int _Pointer, int _AnimL, ref Image[] Ims, Image Bitmap)
        {
            lock (ObjectTicks)
                ObjectTicks.Add(new PersonDrawTickImage(_p, _AnimL, ref Ims, Bitmap, _Pointer));
        }
        public void AddToObjectTicksList(int ControlWidth, int ControlHeight, Image Bitmap, Bullet _Bul)
        {
            lock (ObjectTicks)
                ObjectTicks.Add(new BulletDrawTickImage(ControlWidth, ControlHeight, Bitmap, _Bul));
        }
    }
}
