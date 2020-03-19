using System;
using ObjectClasses;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {


                Person p = new Person()
                {
                    X = r.Next(0, 154165),
                    Y = r.Next(0, 154165),
                    XSpeed = r.Next(0, 154165),
                    YSpeed = r.Next(0, 154165)
                };
                Console.WriteLine(p.GetHashCode());
            }


        }
    }
}
