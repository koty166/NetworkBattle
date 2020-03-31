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


namespace Network_Battle
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Image[] Anims;
        Person LocalPerson , LocalEnemy;///
        ObjectDrawer ObjDraw;
        IntersectController IC;
        PackageReciever InNetConnect;
        PackageSender OutNetConnect;
        List<string> Animations = new List<string>();
        List<int> AnimationAddr = new List<int>() { 0 };
        List<Person> PersonList = new List<Person>();
        byte LocalPersonID;

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

        void AddNetObject(object Package,bool IsBullet)
        {
            if (IsBullet)
            {
                BulletNetDataPackage Data = (BulletNetDataPackage)Package;
                Person Per = GetPersonByID(Data.ParentPersonID);

                if (Per == null)
                    return;

                Bullet b = new Bullet(Per)
                {
                    Curner = Data.Curner,
                    X = Data.X,
                    Y = Data.Y
                };

                ObjDraw.AddToObjectTicksList(Width, Height, BattleField.Image, b);

            }
            else
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
                }

                Per.X = Data.X;
                Per.Y = Data.Y;
                Per.XSpeed = Data.XSpeed;
                Per.YSpeed = Data.YSpeed;

                ObjDraw.IsPersonInList(Per,true);
                ObjDraw.AddToObjectTicksList(Per, Data.AnimAddr, Data.AnimLenght, ref Anims, BattleField.Image);

            }
        }

        void GetPointToDrawBullet(ref double X, ref double Y , double Curner)
        {
            const int Speed = 30;
            double GetRadian(double InCurner) => InCurner * Math.PI / 180;

            if (Curner < 90)
            {
                X += Math.Sin(GetRadian(Curner)) * Speed;
                Y -= Math.Cos(GetRadian(Curner)) * Speed;
            }
            else if (Curner > 90 && Curner < 180)
            {
                X += Math.Cos(GetRadian(Curner - 90)) * Speed;
                Y += Math.Sin(GetRadian(Curner - 90)) * Speed;
            }
            else if (Curner > 180 && Curner < 270)
            {
                X -= Math.Sin(GetRadian(Curner - 180)) * Speed;
                Y += Math.Cos(GetRadian(Curner - 180)) * Speed;
            }
            else if (Curner > 270)
            {
                X -= Math.Cos(GetRadian(Curner - 270)) * Speed;
                Y -= Math.Sin(GetRadian(Curner - 270)) * Speed;
            }
            else if (Curner == 0)
                Y -= 48;
            else if (Curner == 90)
                X += 48;
            else if (Curner == 180)
                Y += 48;
            else if (Curner == 270)
                X -= 48;

        }

        void ShowShoot(int X, int Y, Person p)
        {
            double LCurner = 0;
            double OX, OY;
            lock(p)
            {
                p.X += 48;
                p.Y += 48;
            }
            p.YSpeed = 0;
            p.XSpeed = 0;
            ObjDraw.IsPersonInList(p, true);
            if (X > p.X && Y < p.Y)
            {
                LCurner = Math.Atan2((X - p.X), (p.Y - Y)) * 180 / Math.PI;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[7], 10, ref Anims, BattleField.Image);//n-e
            }
            else if (X > p.X && Y > p.Y)
            {
                LCurner = 180 - Math.Atan2((X - p.X), (Y - p.Y)) * 180 / Math.PI;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[1], 10, ref Anims, BattleField.Image);//s-e
            }
            else if (X < p.X && Y > p.Y)
            {
                LCurner = Math.Atan2((p.X - X), (Y - p.Y)) * 180 / Math.PI + 180;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[3], 10, ref Anims, BattleField.Image);//s-w
            }
            else if (X < p.X && Y < p.Y)
            {
                LCurner = 360 - Math.Atan2((p.X - X), (p.Y - Y)) * 180 / Math.PI;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[5], 10, ref Anims, BattleField.Image);//n-w
            }
            else if (X < p.X && Y == p.Y)
            {
                LCurner = 270;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[4], 10, ref Anims, BattleField.Image);//w
            }
            else if (X > p.X && Y == p.Y)
            {
                LCurner = 90;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[0], 10, ref Anims, BattleField.Image);//e
            }
            else if (X == p.X && Y > p.Y)
            {
                LCurner = 180;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[2], 10, ref Anims, BattleField.Image);//s
            }
            else if (X == p.X && Y < p.Y)
            {
                LCurner = 0;

                ObjDraw.AddToObjectTicksList(p, AnimationAddr[6], 10, ref Anims, BattleField.Image);//n
            }

            OX = p.X;
            OY = p.Y;

            lock (p)
            {
                p.X -= 48;
                p.Y -= 48;
            }

            GetPointToDrawBullet(ref OX, ref OY, LCurner);

            OutNetConnect.SendBullet(new Bullet()
            {
                X = OX,
                Y = OY,
                Curner = (int)LCurner,
                ParentPerson = GetPersonByID(LocalPersonID)
            });
            ObjDraw.AddToObjectTicksList(Width, Height, BattleField.Image, new Bullet(LocalPerson)
            {
                X = OX,
                Y = OY,
                Curner = (int)LCurner
            });
           
            

            ////////////////// Надо убрать преобразование в радианы и обратно
            //ибо смысла в них нет
        }

        void AddToDrList(Person _p, int _AnimL, ref Image[] Ims, Image Bitmap, int _Pointer)
        {
            _p.XSpeed = 0;
            _p.YSpeed = 0;
            ObjDraw.AddToObjectTicksList(_p, _Pointer, _AnimL, ref Ims, Bitmap);
        }

        private void BattleField_MouseClick(object sender, MouseEventArgs e)
        {
            ShowShoot(e.X, e.Y, LocalPerson);
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
            LocalPerson = new Person()
            {
                X = Size.Width / 2,
                Y = Size.Height / 2,
            };
            int Lenght = Dns.GetHostAddresses(Dns.GetHostName()).Length;
            LocalPerson.ID = byte.Parse(Dns.GetHostAddresses(Dns.GetHostName())[0].MapToIPv4().ToString().Split('.')[3]);
            LocalPersonID = LocalPerson.ID;

            LocalEnemy = new Person()
            {
                X = Width / 3,
                Y = Height / 3
            };


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
            //////////////
            ObjDraw.AddToObjectTicksList(LocalEnemy, AnimationAddr[7], -1, ref Anims, BattleField.Image);
            PersonList.Add(LocalPerson);
            /////////////////
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BattleField.Refresh();
            // BattleField.Invalidate /////////////may be this?
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
            OutNetConnect.SendingIniPackage();
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

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            int Speed = 5;
            ObjDraw.IsPersonInList(LocalPerson, true);
            switch (e.KeyCode)
            {
                case Keys.W:
                    LocalPerson.YSpeed = -Speed;
                    LocalPerson.XSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[8], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(LocalPerson,AnimationAddr[8], 6,true);
                    break;
                case Keys.A:
                    LocalPerson.XSpeed = -Speed;
                    LocalPerson.YSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[9], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(LocalPerson, AnimationAddr[9], 6, true);
                    break;
                case Keys.S:
                    LocalPerson.YSpeed = Speed;
                    LocalPerson.XSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[10], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(LocalPerson, AnimationAddr[10], 6, true);
                    break;
                case Keys.D:
                    LocalPerson.XSpeed = Speed;
                    LocalPerson.YSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[11], 6, ref Anims, BattleField.Image);
                    OutNetConnect.SendPerson(LocalPerson, AnimationAddr[11], 6, true);
                    break;
                case Keys.P:
                    Person P = new Person()
                    {
                        ID = 0,
                        X = 45,
                        Y = 45,
                        XSpeed = 0,
                        YSpeed = 0
                    };
                    OutNetConnect.SendPerson(P, 0, 6, true);
                    break;
            }
          
        }
    }
}
