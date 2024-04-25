using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BitStringEncoding
{
    public class BitSequence
    {
        public byte[] value { get; set; }
        public ulong bits { get; set; }
        

        public BitSequence(BitSequence bitSequence)
        {
            this.value = bitSequence.value;
            this.bits = bitSequence.bits;
        }
        public BitSequence(ulong bits)
        {
            this.value = new byte[(bits - (bits % 8)) / 8 + 1];
            this.bits = bits;
        }
        public BitSequence(bool[] boolInput)
        {
            byte[] output = new byte[(boolInput.Length - (boolInput.Length % 8)) / 8 + 1];

            for (ulong i = 0; i < (ulong)boolInput.Length; i++)
            {
                output.SetBitInByte(i, boolInput[i]);
            }

            this.value = output;
            this.bits = (ulong)boolInput.Length;
        }
    }



    public static class BitSequenceExtensions
    {
        public static BitSequence SetBitInSequence(this BitSequence bitSequence, ulong index, bool value)
        {
            index = Math.Min(bitSequence.bits, index);

            bitSequence.value = bitSequence.value.SetBitInByte(index, value);

            return bitSequence;
        }

        public static BitSequence AddOne(this BitSequence bitSequence, bool value)
        {
            if (bitSequence.bits < 8 + bitSequence.bits - (bitSequence.bits % 8))
            {
                bitSequence.value = bitSequence.value.AddByte().SetBitInByte(bitSequence.bits, value);
            }
            else
            {
                bitSequence.value = bitSequence.value.SetBitInByte(bitSequence.bits, value);
            }
            bitSequence.bits++;

            return bitSequence;
        }
        public static BitSequence AddTwo(this BitSequence bitSequence, bool value)
        {
            if (bitSequence.bits < 8 + bitSequence.bits - (bitSequence.bits % 8))
            {
                bitSequence.value = bitSequence.value.AddByte().SetBitInByte(bitSequence.bits, value);
            }
            else
            {
                bitSequence.value = bitSequence.value.SetBitInByte(bitSequence.bits, value);
            }
            bitSequence.bits++;

            return bitSequence;
        }

        // Make a function to return a byte[] or bool[] of the values in a range
        //public static BitSequence Range(this BitSequence bitSequence, int rangeStart, int rangeEnd)
        //{
        //    BitSequence output = 
        //}
    }



    public static class ByteExtensions
    {
        public static byte SetBit(this byte inputByte, int index, bool value) // Sets a bit within a byte to a specified value
        {
            index = Math.Clamp(index, 0, 7);
            if (value)
            {
                return inputByte |= (byte)(1 << (7 - index));
            }
            else
            {
                return inputByte &= (byte)((1 << (7 - index)) ^ 0xFF);
            }
        }



        public static byte[] SetBitInByte(this byte[] inputBytes, ulong index, bool value) 
        {
            int bitIndex = (int)(index % 8);
            ulong byteIndex = (index - (ulong)bitIndex) / 8;
            inputBytes[byteIndex] = inputBytes[byteIndex].SetBit(bitIndex, value);

            return inputBytes;
        }



        public static byte[] AddByte(this byte[] inputBytes, bool value = false)
        {
            byte[] output = new byte[inputBytes.Length + 1];

            for (int i = 0; i < inputBytes.Length; i++)
            {
                output[i] = inputBytes[i];
            }

            if (value)
            {
                output[inputBytes.Length] = byte.MaxValue;
            }
            else
            {
                output[inputBytes.Length] = byte.MinValue;
            }

            return output;
        }
        public static byte[] AddByteTwo(this byte[] inputBytes, bool value = false)
        {
            byte[] output = new byte[inputBytes.Length + 1];

            Buffer.BlockCopy(inputBytes, 0, output, 0, inputBytes.Length);

            if (value)
            {
                output[inputBytes.Length] = byte.MaxValue;
            }
            else
            {
                output[inputBytes.Length] = byte.MinValue;
            }

            return output;
        }
        public static byte[] AddByteThree(this byte[] inputBytes, bool value = false)
        {
            byte[] output = new byte[inputBytes.Length + 1];

            Array.Copy(inputBytes, output, inputBytes.Length);

            if (value)
            {
                output[inputBytes.Length] = byte.MaxValue;
            }
            else
            {
                output[inputBytes.Length] = byte.MinValue;
            }

            return output;
        }
        public static byte[] AddByteFour(this byte[] inputBytes, bool value = false)
        {
            byte[] output = new byte[inputBytes.Length + 1];

            Array.Resize(ref inputBytes, inputBytes.Length + 1);

            if (value)
            {
                output[inputBytes.Length] = byte.MaxValue;
            }
            else
            {
                output[inputBytes.Length] = byte.MinValue;
            }

            return output;
        }
    }
}
