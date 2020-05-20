using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectClasses
{
   public class Person
    {
        public float X, Y, XSpeed, YSpeed;
        public byte ID;
        public int DeadNum = 0;
        public int CurrentAnimAddr, IdleAnimAddr,MaxAnimAddr;
        public bool IsIdle = false;

        public const float SizeX = 0.5f, SizeY = 0.5f;
    }
}
