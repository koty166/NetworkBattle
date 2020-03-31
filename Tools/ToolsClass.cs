using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class ToolsClass
    {
        public static void CopyFromArrayToArry(ref byte[] FirstArray, ref byte[] SecoundArray, int PointerA, int PointerB, int Lenght)
        {
            for (int i = PointerB; i < Lenght + PointerB; i++)
            {
                FirstArray[PointerA] = SecoundArray[i];
                PointerA++;
            }
        }
    }
}
