using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectClasses
{
   public class Person
    {
        public int X, Y , XSpeed , YSpeed;

        public List<string> Animations = new List<string>();
        public List<int> AnimationAddr = new List<int>() { 0 };
    }
}
