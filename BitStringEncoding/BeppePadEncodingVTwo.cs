using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BitStringEncoding
{
    internal class BeppePadEncodingVTwo
    {
        public void BitWiseOperations()
        {
            byte myByte = 0b11111111;
            Console.WriteLine("Function test");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine(Convert.ToString(myByte.SetBit(i, false), 2) + " " + myByte.SetBit(i, false));
            }


            

        }
    }
}
