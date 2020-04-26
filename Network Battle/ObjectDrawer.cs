using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using ObjectClasses;
using Events;

namespace Network_Battle
{
   public class ObjectDrawer : IDisposable
    {
        internal List<ObjectForDraw> ObjectTicks;
        Thread ThObjectDrawer;

        public ObjectDrawer(Image Bitmap,EventsClass.AddToDrawList EvNeedToAddStaticPicture , SynchronizationContext SynContext)
        {
            ObjectTicks = new List<ObjectForDraw>();

            ThObjectDrawer = new Thread(new ParameterizedThreadStart(Draw));

            object[] m = new object[4];
            m[0] = Bitmap;
            m[1] = ObjectTicks;
            m[2] = EvNeedToAddStaticPicture;
            m[3] = SynContext;

            ThObjectDrawer.Start(m);
        }

        static void Draw(object m)
        {
            Graphics g;
            lock ((Image)((object[])m)[0])
                g = Graphics.FromImage((Image)((object[])m)[0]);

            List<ObjectForDraw> ListObj = (List<ObjectForDraw>)((object[])m)[1];
            EventsClass.AddToDrawList EvNeedToAddStaticPicture = ((EventsClass.AddToDrawList)((object[])m)[2]);
            SynchronizationContext SynContext = ((SynchronizationContext)((object[])m)[3]);

            Image BattleFieldIm = Image.FromFile("Resourses\\Battle field.bmp");
            MainWindow MainWin = (MainWindow)Application.OpenForms[0];

            int Pause = 25;
            const int MaxTicks = 4;
            int Ticks = 1;
            bool IsNeedToDraw = true;

            while (true)
            { 
                g.DrawImage(BattleFieldIm,0,0);
                //g.Clear(Color.AntiqueWhite);
                lock (ListObj)
                {
                    foreach (var i in ListObj)
                    {
                        if (i is PersonDrawTickImage)
                            ((PersonDrawTickImage)i).Draw(IsNeedToDraw);
                        else
                            ((BulletDrawTickImage)i).Draw(IsNeedToDraw);
                    }

                    for (int i = 0; i < ListObj.Count; i++)
                        if (ListObj[i].IsNeedToDestroy)
                        {
                            if (ListObj[i] is PersonDrawTickImage)
                                ((PersonDrawTickImage)ListObj[i]).InvokeEventForAddToDrawList(EvNeedToAddStaticPicture);
                            ListObj.RemoveAt(i);
                        } 
                }
                Thread.Sleep(Pause);

                SynContext.Send(_ => MainWin.BattleField.Refresh(),null);

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

        public bool IsPersonInList(int ID , bool IsNeedToRemove)
        {
            lock (ObjectTicks)
            {
                for (int i = 0; i < ObjectTicks.Count; i++)
                {
                    if (ObjectTicks[i] is PersonDrawTickImage && ((PersonDrawTickImage)ObjectTicks[i])._Person.ID == ID)
                    {
                        if (IsNeedToRemove)
                        {
                            ObjectTicks.RemoveAt(i);
                        }
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
