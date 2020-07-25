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
using SharpGL;
using SharpGL.SceneGraph.Assets;
using SharpGLDrawLibrary;
using System.Text;

namespace Network_Battle
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Texture[] Anims;
        PackageReciever InNetConnect;
        PackageSender OutNetConnect;
        List<string> Animations = new List<string>();
        List<int> AnimationAddr = new List<int>() { 0 };
        List<Person> PersonList = new List<Person>();
        List<Bullet> BulletList = new List<Bullet>();

        public event EventsClass.PackageSave PackgeWasGot;
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

        void AddNetObject(object Package)
        {
            NetDataPackage Data = (NetDataPackage)Package;
            Person Per;
            //if (Data.)
           //     return;

            if (Data.ID == 0)
            {
                StringBuilder RemoveAddr = new StringBuilder(20);
                foreach (var item in Data.SourseIp)
                {
                    RemoveAddr.Append(item);
                    RemoveAddr.Append('.');
                }
                OutNetConnect.AddToAddrIPList(RemoveAddr.ToString());
                return;
            }


            object PrePerson = GetPersonByID(Data.ID);

            Per = PrePerson == null ? new Person() { ID = Data.ID} : (Person)PrePerson;

            Per.X = Data.X;
            Per.Y = Data.Y;
            Per.XSpeed = Data.XSpeed;
            Per.YSpeed = Data.YSpeed;

            if (PrePerson == null)
                PersonList.Add(Per);

            if (Data.BulletCurner != 10)
                AddBullet(Data.BulletCurner, Per.X + 48, Per.Y + 48, Per);
        }

        void AddBullet(float Curner, float X, float Y, Person Parent) => BulletList.Add( new Bullet(Parent)
        {
            X = X,
            Y = Y,
            Curner = Curner
        });

        
        void ShowShoot(int X, int Y, Person p)
        {
            int AnimAddr;
            float BulX = p.X + Person.SizeX / 2, BulY = p.Y + Person.SizeY / 2;
            p.XSpeed = 0;
            p.YSpeed = 0;
            float Curner = ToolsClass.CountCurner(X, Y, p.X, p.Y, out AnimAddr);

           p.ChangeAnim(AnimationAddr[AnimAddr] , AnimationAddr[AnimAddr + 1]);

            OutNetConnect.SendPerson(p, (int)(Curner * 180 / Math.PI));
            AddBullet(Curner, BulX, BulY, p);
        }

        Texture[] LoadAnimations(OpenGL gl, string[] AnimationPath)
        {
            Texture[] Ims = new Texture[AnimationPath.Length];

            for (int i = 0; i < AnimationPath.Length; i++)
            {
                Ims[i] = new Texture();
                Ims[i].Create(gl, (Bitmap)Image.FromFile(AnimationPath[i]));
            }

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

            this.DesktopLocation = new Point(0, 0);
            ////////////////////////////////
            PackgeWasGot += AddNetObject;

            // ObjDraw = new ObjectDrawer(BattleField.Image, EventAddToDrawList, SynchronizationContext.Current);
            // IC = new IntersectController(ObjDraw);
            OutNetConnect = new PackageSender();

            //  AddToNetAddrList += OutNetConnect.AddToAddrIPList;
            //  InNetConnect = new PackageReciever(PackgeWasGot, PersonList, AddToNetAddrList);

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

            Anims = LoadAnimations(openGLControl1.OpenGL, Animations.ToArray());

            PersonList.Add(new Person()
            {
                X = 0,
                Y = 0,
                ID = byte.Parse(Dns.GetHostAddresses(Dns.GetHostName())[Dns.GetHostAddresses(Dns.GetHostName()).Length - 1].ToString().Split('.')[3]),
                IdleAnimAddr = AnimationAddr[0],
                MaxAnimAddr = AnimationAddr[1],
                CurrentAnimAddr = 0,
            });

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // IC.Dispose();
            // InNetConnect.Dispose();
            // InNetConnect.DisposeTcpClient();
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
           /* string s = "";
            foreach (var i in PersonList)
            {
                s += i.ID + ":" + i.DeadNum + "\n";
            }
            MessageBox.Show(s);*/
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            float Speed = 0.08f;

            PersonList[0].YSpeed = 0;
            PersonList[0].XSpeed = 0;
            switch (e.KeyCode)
            {
                case Keys.W:
                    PersonList[0].YSpeed = Speed;
                    ToolsClass.ActivateAnimation(PersonList[0],AnimationAddr[6], AnimationAddr[8] +5, AnimationAddr[8]);

                    // ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[8], 6, ref Anims, BattleField.Image);
                    // OutNetConnect.SendPerson(PersonList[0], AnimationAddr[8], 6,false,-1);
                    break;
                case Keys.A:
                    PersonList[0].XSpeed = Speed;
                    ToolsClass.ActivateAnimation(PersonList[0], AnimationAddr[4], AnimationAddr[9] +5, AnimationAddr[9]);
                    // ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[9], 6, ref Anims, BattleField.Image);
                    // OutNetConnect.SendPerson(PersonList[0], AnimationAddr[9], 6, false,-1);
                    break;
                case Keys.S:
                    PersonList[0].YSpeed = -Speed;
                    ToolsClass.ActivateAnimation(PersonList[0], AnimationAddr[2], AnimationAddr[10] +5, AnimationAddr[10]);
                    //  ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[10], 6, ref Anims, BattleField.Image);
                    //OutNetConnect.SendPerson(PersonList[0], AnimationAddr[10], 6, false,-1);
                    break;
                case Keys.D:
                    PersonList[0].XSpeed = -Speed;
                    ToolsClass.ActivateAnimation(PersonList[0], AnimationAddr[0], AnimationAddr[11] + 5, AnimationAddr[11]);
                    // ObjDraw.AddToObjectTicksList(PersonList[0], AnimationAddr[11], 6, ref Anims, BattleField.Image);
                    //OutNetConnect.SendPerson(PersonList[0], AnimationAddr[11], 6, false,-1);
                    break;
            }


        }

        void UpdateScreen(OpenGL gl)
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            gl.LookAt(X, Y, Z,    // Позиция самой камеры (x, y, z)
                        X, Y, 0,     // Направление, куда мы смотрим
                        0, 2, 0);    // Верх камеры
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        Texture Field;
        OpenGL gl;
        private void openGLControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
        {

            gl.Clear(OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_COLOR_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Color(1.0f, 1.0f, 1.0f, 0.1f);


            Drawer.DrawImageByGl(gl, Field, 0, 0, -5, -5);
            foreach (var i in PersonList)
            {
                gl.Enable(OpenGL.GL_BLEND);
                //gl.BlendFunc(1, 0);

                Drawer.DrawImageByGl(gl, Anims[i.CurrentAnimAddr], i.X, i.Y, -0.5f, -0.5f, -0.5f);
                gl.Disable(OpenGL.GL_BLEND);
            }
            foreach (var i in BulletList)
            {
                Drawer.DrawCIRCLE(gl,Color.Red,i.X,i.Y,0);
            }
            gl.Flush();
        }

        double X = -1.1, Y = -1.15, Z = -4;

        private void PersonImageMaker_Tick(object sender, EventArgs e)
        {
            foreach (var i in PersonList)
            {
                i.X += i.XSpeed;
                i.Y += i.YSpeed;

                if (!i.IsIdle)
                    i.CurrentAnimAddr++;
                if (i.CurrentAnimAddr == i.MaxAnimAddr)
                {
                    i.CurrentAnimAddr = i.IdleAnimAddr;
                    i.IsIdle = true;

                    i.YSpeed = 0;
                    i.XSpeed = 0;
                }
            }
            foreach (var i in BulletList)
            {
                i.Tick(new Rectangle(-10,-10,20,20));
            }


            }

        private void openGLControl1_MouseClick(object sender, MouseEventArgs e)
        {
            ShowShoot(e.X, e.Y, PersonList[0]);
        }

        private void openGLControl1_Resized(object sender, EventArgs e) =>
            UpdateScreen(gl);

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            gl = openGLControl1.OpenGL;
            Field = new Texture();
            Field.Create(gl, "field.bmp");

            //gl.ClearColor(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B, Color.Yellow.A);
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            UpdateScreen(gl);

        }
    }
}
