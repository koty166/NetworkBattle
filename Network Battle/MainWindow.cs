using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using ObjectClasses;
using Events;
using NetworkLibrary;
using Tools;


namespace Network_Battle
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Image[] Anims;
        //Person LocalPerson;///
        ObjectDrawer ObjDraw;
        IntersectController IC;
        PackageReciever InNetConnect;
        PackageSender OutNetConnect;
        List<string> Animations = new List<string>();
        List<int> AnimationAddr = new List<int>() { 0 };
        List<Person> PersonList = new List<Person>();

        public event EventsClass.PackageSave PackgeWasGot;
        public event EventsClass.AddToDrawList EventAddToDrawList;
        public event EventsClass.AddToAddrList AddToNetAddrList;

        Person GetPersonByID(int ID)
        {
            foreach (var i in PersonList)
            {
                if (i.ID == ID)
                    return i;
            }
            return null;
        }

        bool IsPerInColl(byte ID)
        {
            foreach (var i in PersonList)
            {
                if (i.ID == ID)
                    return true;

            }
            return false;

        }

        void AddNetObject(object Package)
        {
            PersonNetDataPackage Data = (PersonNetDataPackage)Package;
            Person Per;
            if (Data.PersonID == -2)
                return;

            else if (Data.PersonID == -1)
            {
                string RemoveAddr = Data.X.ToString() + "." + Data.Y.ToString() + "." + Data.XSpeed.ToString() + "." + Data.YSpeed.ToString();
                OutNetConnect.AddToAddrIPList(RemoveAddr);
                return;
            }

            if (!Data.IsNewPerson)
            {
                Per = GetPersonByID(Data.PersonID);

                if (Per == null)
                    return;
            }
            else
            {
                Per = new Person();
                Per.ID = (byte)Data.PersonID;
                if (!IsPerInColl(Per.ID))
                    PersonList.Add(Per);

            }

            Per.X = Data.X;
            Per.Y = Data.Y;
            Per.XSpeed = Data.XSpeed;
            Per.YSpeed = Data.YSpeed;

            if (Data.BulletCurner != -1)
                AddBullet(Data.BulletCurner  * Math.PI / 180, Per.X + 48, Per.Y + 48, Per);

            ObjDraw.IsPersonInList(Per.ID, true);
            ObjDraw.AddToObjectTicksList(Per, Data.AnimAddr, Data.AnimLenght, ref Anims, BattleField.Image);

        }

        void AddBullet(double Curner, int X, int Y, Person Parent) => ObjDraw.AddToObjectTicksList(Width, Height, BattleField.Image, new Bullet(Parent)
        {
            X = X,
            Y = Y,
            Curner = Curner,
            ParentPerson = Parent
        });

        void ShowShoot(int X, int Y, Person p)
        {
            ObjDraw.IsPersonInList(p.ID, true);
            int AnimAddr, BulX = p.X + 48, BulY = p.Y + 48;
            double Curner = ToolsClass.CountCurner(X, Y, p.X, p.Y, out AnimAddr);

            ObjDraw.AddToObjectTicksList(new Person() {X = p.X,Y = p.Y,XSpeed = 0, YSpeed = 0, ID = p.ID }, AnimationAddr[AnimAddr], 10, ref Anims, BattleField.Image);
            OutNetConnect.SendPerson(new Person() { ID = p.ID, X = p.X, Y = p.Y, XSpeed = 0, YSpeed = 0 }, AnimationAddr[AnimAddr], 10, false, (int)(Curner * 180 / Math.PI));
            AddBullet(Curner,BulX,BulY,p);
        }

        void AddToDrList(Person _p, int _AnimL, ref Image[] Ims, Image Bitmap, int _Pointer)
        {
            _p.XSpeed = 0;
            _p.YSpeed = 0;
            ObjDraw.AddToObjectTicksList(_p, _Pointer, _AnimL, ref Ims, Bitmap);
        }

        private void BattleField_MouseClick(object sender, MouseEventArgs e)
        {
            ShowShoot(e.X, e.Y, PersonList[0]);
        }

        Image[] LoadAnimations(string[] AnimationPath)
        {
            Image[] Ims = new Image[AnimationPath.Length];

            for (int i = 0; i < AnimationPath.Length; i++)
                Ims[i] = Image.FromFile(AnimationPath[i]);

            return Ims;
        }

        void AddToAnimationList(string[] Paths)
        {
            foreach (var i in Paths)
            {
                Animations.Add(i);
            }
            AnimationAddr.Add(Animations.Count);
            
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            ///////////////////////////////////////////
            PersonList.Add(new Person()
            {
                X = Size.Width / 2,
                Y = Size.Height / 2,
                ID = byte.Parse(Dns.GetHostAddresses(Dns.GetHostName())[Dns.GetHostAddresses(Dns.GetHostName()).Length - 1].ToString().Split('.')[3])
            });

            ////////////////////////////////
            EventAddToDrawList += AddToDrList;
            PackgeWasGot += AddNetObject;

            ObjDraw = new ObjectDrawer(BattleField.Image, EventAddToDrawList, SynchronizationContext.Current);
            IC = new IntersectController(ObjDraw);
            OutNetConnect = new PackageSender();

            AddToNetAddrList += OutNetConnect.AddToAddrIPList;
            InNetConnect = new PackageReciever(PackgeWasGot, PersonList, AddToNetAddrList);

            ////////////////////////////////////////////////
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting e0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting se0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting s0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting sw0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting w0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting nw0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting n0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting ne0*.bmp"));

            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking n0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking w0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking s0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking e0*.bmp"));

            Anims = LoadAnimations(Animations.ToArray());
            

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjDraw.Dispose();
            IC.Dispose();
            InNetConnect.Dispose();
            InNetConnect.DisposeTcpClient();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OutNetConnect.SendingIniPackage(PersonList[0]);
        }

        private void СписокАдресовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (var i in OutNetConnect.GetAddrList())
            {
                s += i.Address + ":" + i.Port + "\n";
            }
            MessageBox.Show(s);
        }

        private void показатьСписокПерсонажейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(PersonList.Count.ToString());
        }

        private void локальныйАдрессToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (var i in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                s += i + ":" + i.AddressFamily.ToString() + "\n";
            }
            MessageBox.Show(s);
        }

        private void списокСмертейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "";
            foreach (var i in PersonList)
            {
                s += i.ID + ":" + i.DeadNum + "\n";
            }
            MessageBox.Show(s);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            int Speed = 5;
            ObjDraw.IsPersonInList(PersonList[0].ID, true);
            switch (e.KeyCode)
            {
                case Keys.W:
                    PersonList[0].YSpeed = -Speed;
                    PersonList[0].XSpeed = 0;
                    ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[8], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(PersonList[0], AnimationAddr[8], 6,false,-1);
                    break;
                case Keys.A:
                    PersonList[0].XSpeed = -Speed;
                    PersonList[0].YSpeed = 0;
                    ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[9], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(PersonList[0], AnimationAddr[9], 6, false,-1);
                    break;
                case Keys.S:
                    PersonList[0].YSpeed = Speed;
                    PersonList[0].XSpeed = 0;
                    ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[10], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(PersonList[0], AnimationAddr[10], 6, false,-1);
                    break;
                case Keys.D:
                    PersonList[0].XSpeed = Speed;
                    PersonList[0].YSpeed = 0;
                    ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[11], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(PersonList[0], AnimationAddr[11], 6, false,-1);
                    break;
            }
          
        }
    }
}
