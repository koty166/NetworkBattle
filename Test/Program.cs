using System;
using ObjectClasses;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (var i in BitConverter.GetBytes(true))
            {
                Console.Write(i.ToString() + " ");
            }
            
           


        }
    }
}
