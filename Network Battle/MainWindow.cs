using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using ObjectClasses;
using Events;


namespace Network_Battle
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Image[] Anims;
        Person LocalPerson , LocalEnemy;
        ObjectDrawer ObjDraw;
        IntersectController IC;
        List<string> Animations = new List<string>();
        List<int> AnimationAddr = new List<int>() { 0 };
        List<Person> PersonList = new List<Person>();

        public event EventsClass.PackageSave PackgeWasGot;

        public event EventsClass.AddToDrawList EventAddToDrawList;

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
                if (!Data.IsNewPerson)
                {
                    Per = GetPersonByID(Data.PersonID);

                    if (Per == null)
                        return;
                }
                else
                {
                    Per = new Person();
                    Per.ID = Data.PersonID;
                }

                Per.X = Data.X;
                Per.Y = Data.Y;
                Per.XSpeed = Data.XSpeed;
                Per.YSpeed = Data.YSpeed;

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
            Random r = new Random();
            double OX, OY;
            lock(p)
            {
                p.X += 48;
                p.Y += 48;
            }
            if (X > p.X && Y < p.Y)
            {
                LCurner = Math.Atan2((X - p.X), (p.Y - Y)) * 180 / Math.PI;
            }
            else if (X > p.X && Y > p.Y)
            {
                LCurner = 180 - Math.Atan2((X - p.X), (Y - p.Y)) * 180 / Math.PI;
            }
            else if (X < p.X && Y > p.Y)
            {
                LCurner = Math.Atan2((p.X - X), (Y - p.Y)) * 180 / Math.PI + 180;
            }
            else if (X < p.X && Y < p.Y)
            {
                LCurner = 360 - Math.Atan2((p.X - X), (p.Y - Y)) * 180 / Math.PI;
            }
            else if (X < p.X && Y == p.Y)
            {
                LCurner = 270;
            }
            else if (X > p.X && Y == p.Y)
            {
                LCurner = 90;
            }
            else if (X == p.X && Y > p.Y)
            {
                LCurner = 180;
            }
            else if (X == p.X && Y < p.Y)
            {
                LCurner = 0;
            }

            OX = p.X;
            OY = p.Y;

            lock (p)
            {
                p.X -= 48;
                p.Y -= 48;
            }

            GetPointToDrawBullet(ref OX, ref OY, LCurner);

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
                Y = Size.Height / 2
            };
            LocalEnemy = new Person()
            {
                X =  Width / 3,
                Y =  Height / 3
            };


            ////////////////////////////////
            EventAddToDrawList += AddToDrList;
            PackgeWasGot += AddNetObject;

            ObjDraw = new ObjectDrawer(BattleField.Image, EventAddToDrawList);
            IC = new IntersectController(ObjDraw);
            ////////////////////////////////////////////////
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting e0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting se0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting s0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting sw0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting w0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting nw0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "shooting n0*.bmp"));

            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking n0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking w0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking s0*.bmp"));
            AddToAnimationList(Directory.GetFiles("Resourses//Person//", "walking e0*.bmp"));

            Anims = LoadAnimations(Animations.ToArray());
//////////////
            ObjDraw.AddToObjectTicksList(LocalEnemy,AnimationAddr[7],-1,ref Anims, BattleField.Image);
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
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[7], 6, ref Anims, BattleField.Image);
                    break;
                case Keys.A:
                    LocalPerson.XSpeed = -Speed;
                    LocalPerson.YSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[8], 6, ref Anims, BattleField.Image);
                    break;
                case Keys.S:
                    LocalPerson.YSpeed = Speed;
                    LocalPerson.XSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[9], 6, ref Anims, BattleField.Image);
                    break;
                case Keys.D:
                    LocalPerson.XSpeed = Speed;
                    LocalPerson.YSpeed = 0;
                    ObjDraw.AddToObjectTicksList(LocalPerson, AnimationAddr[10], 6, ref Anims, BattleField.Image);
                    break;   
            }
          
        }
    }
}
