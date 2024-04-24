using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitStringEncoding
{
    internal class OptimalByteStructureVTwo
    {
        int maxByteLength = 22;
        int minByteLength = 1;
        int minPadding = 1;

        bool totalBitOptimal = false;
        bool integerOptimal = true;
        bool utfEightOptimal = false;

        bool breakAtFirst = true;

        public void FindOptimalSettings(int targetValue, bool breakAtFirst = true, int byteLengthChecks = 2, bool totalBitOptimal = false, bool integerOptimal = true, bool utfEightOptimal = false)
        {
            minByteLength = (int)Math.Ceiling(Math.Log2(targetValue));
            maxByteLength = minByteLength + byteLengthChecks;

            this.totalBitOptimal = totalBitOptimal;
            this.integerOptimal = integerOptimal;
            this.utfEightOptimal = utfEightOptimal;

            this.breakAtFirst = breakAtFirst;
        }

        public int[] FindOptimalStructure(int targetValue)
        {
            Console.Clear();
            Console.WriteLine("Target: " + targetValue);



            Dictionary<int, List<int>> output = new Dictionary<int, List<int>>();

            BestPickTwo bestPick = new BestPickTwo(0, 0, ulong.MaxValue, ulong.MinValue, new Dictionary<int, int>(), 0, false, false, false);

            BestPickTwo bestBitPick = new BestPickTwo(0, 0, ulong.MaxValue, ulong.MinValue, new Dictionary<int, int>(), 0, false, false, false);
            BestPickTwo profitablePick = new BestPickTwo(0, 0, ulong.MaxValue, ulong.MinValue, new Dictionary<int, int>(), 0, false, false, false);


            int neededBits = (int)Math.Ceiling(Math.Log2(targetValue));
            int NormalByteLen = 8 * (int)Math.Floor(0.125f * (neededBits + 7));
            int integerByteLen = (int)Math.Pow(2, (double)NormalByteLen / 8 + 2);
            //int integerByteLen = 8 * (int)Math.Floor(0.125f * neededBits + 7);
            ulong integerMaxValue = (ulong)Math.Pow(2, integerByteLen);
            ulong integerTotalBits = integerMaxValue * (ulong)integerByteLen;
            //Console.WriteLine(neededBits);
            Console.WriteLine(integerByteLen);
            //Console.WriteLine(integerMaxValue);
            //Console.WriteLine(integerTotalBits);

            for (int i = minByteLength; i < maxByteLength; i++)
            //for (int i = minByteLength; i < 18; i++)
            {
                Console.WriteLine(i);

                int topByteValue = (int)Math.Pow(2, i);

                bool skipItteration = false;
                int skipped = 0;

                //for (int j = 1; j <= i / 2; j++)
                for (int j = 1; j <= i; j++)
                {
                    Dictionary<int, int> byteLenCount = new Dictionary<int, int>();
                    ulong paddingCombinedBits = 0;
                    ulong validCombinations = 0;

                    string padding = "";
                    for (int k = 0; k < j; k++)
                    {
                        padding += "0";
                    }

                    int binaryValue = 0;
                    while (binaryValue <= topByteValue)
                    {
                        string binaryString = Convert.ToString(binaryValue, 2);
                        binaryValue++;


                        if (!binaryString.Contains(padding))
                        {
                            validCombinations++;

                            paddingCombinedBits += (ulong)(j + binaryString.Length);

                            if (paddingCombinedBits > bestPick.totalBits) { skipItteration = true; skipped += 1; break; }

                            if (!byteLenCount.ContainsKey(j + binaryString.Length)) { byteLenCount.Add(j + binaryString.Length, 1); }
                            else { byteLenCount[j + binaryString.Length]++; }

                            if (validCombinations >= (ulong)targetValue) { break; }
                        }
                    }
                    if (skipItteration) { continue; }

                    int paramProfitable = 0;
                    foreach (var number in byteLenCount)
                    {
                        if (number.Key <= integerByteLen)
                        {
                            paramProfitable += number.Value;
                        }
                    }

                    if (totalBitOptimal)
                    {
                        if (validCombinations >= (ulong)targetValue && paddingCombinedBits < bestPick.totalBits)
                        {
                            bestBitPick.paddingLen = j;
                            bestBitPick.byteLen = i;
                            bestBitPick.totalBits = paddingCombinedBits;
                            bestBitPick.validCombinations = validCombinations;
                            bestBitPick.byteLenCount = byteLenCount;
                            bestBitPick.profitable = paramProfitable;

                            bestBitPick.totalBitOptimal = totalBitOptimal;
                            bestBitPick.integerOptimal = integerOptimal;
                            bestBitPick.utfEightOptimal = utfEightOptimal;

                            if (profitablePick != new BestPickTwo(0, 0, ulong.MaxValue, ulong.MinValue, new Dictionary<int, int>(), 0, false, false, false))
                            {
                                bestPick = bestBitPick;
                            }

                            Console.WriteLine($"Bit Optimal\n" +
                                $"Padding: {j}, ByteLen: {i}\n" +
                                $"Valid combinations {validCombinations}, Max combinations {integerMaxValue}, Profitable: {paramProfitable}, Overshoot: {validCombinations - (ulong)targetValue}\n" +
                                $"Padding bits: {paddingCombinedBits}, Static bits {integerTotalBits}, Static byte bits: {integerByteLen}");

                            if (breakAtFirst) { return new int[2] { bestPick.paddingLen, bestPick.byteLen }; }
                        }
                    }
                    if (integerOptimal)
                    {
                        //if (paramProfitable >= targetValue && paddingCombinedBits < bestPick.totalBits)
                        if (validCombinations >= (ulong)targetValue && paramProfitable >= bestPick.profitable && paddingCombinedBits < bestPick.totalBits)
                        {
                            profitablePick.paddingLen = j;
                            profitablePick.byteLen = i;
                            profitablePick.totalBits = paddingCombinedBits;
                            profitablePick.validCombinations = validCombinations;
                            profitablePick.byteLenCount = byteLenCount;
                            profitablePick.profitable = paramProfitable;

                            profitablePick.totalBitOptimal = totalBitOptimal;
                            profitablePick.integerOptimal = integerOptimal;
                            profitablePick.utfEightOptimal = utfEightOptimal;

                            bestPick = profitablePick;

                            Console.WriteLine($"Integer Optimal\n" +
                                $"Padding: {j}, ByteLen: {i}\n" +
                                $"Valid combinations {validCombinations}, Max combinations {integerMaxValue}, Profitable: {paramProfitable}, Overshoot: {validCombinations - (ulong)targetValue}\n" +
                                $"Padding bits: {paddingCombinedBits}, Static bits {integerTotalBits}, Static byte bits: {integerByteLen}");

                            foreach (var byteLen in byteLenCount)
                            {
                                Console.WriteLine($"Bits: {byteLen.Key}, count: {byteLen.Value}");
                            }

                            //if (!output.ContainsKey(targetValue))
                            //{
                            //    output.Add(targetValue, new List<int>());
                            //    output[targetValue].Add(i);
                            //}
                            //else
                            //{
                            //    output[targetValue].Add(i);
                            //}

                            if (breakAtFirst) { return new int[2] { bestPick.paddingLen, bestPick.byteLen }; }
                        }
                    }
                }
                Console.WriteLine("Skipped: " + skipped);
            }
            foreach (var number in bestPick.byteLenCount)
            {
                Console.WriteLine($"Bit: {number.Key}, Count: {number.Value}");
            }
            Console.WriteLine($"Profitable: {bestPick.profitable}");
            Console.WriteLine("Done");
            //return output;
            return new int[2] { bestPick.paddingLen, bestPick.byteLen };
        }
    }

    public class BestPickTwo
    {
        public int paddingLen { get; set; }
        public int byteLen { get; set; }
        public int profitable { get; set; }
        public ulong totalBits { get; set; }
        public ulong validCombinations { get; set; }
        public Dictionary<int, int> byteLenCount { get; set; }

        public bool totalBitOptimal { get; set; }
        public bool integerOptimal { get; set; }
        public bool utfEightOptimal { get; set; }

        public BestPickTwo(int paddingLen, int byteLen, ulong totalBits, ulong validCombinations, Dictionary<int, int> byteLenCount, int profitable, bool totalBitOptimal, bool integerOptimal, bool utfEightOptimal)
        {
            this.paddingLen = paddingLen;
            this.byteLen = byteLen;
            this.totalBits = totalBits;
            this.validCombinations = validCombinations;
            this.byteLenCount = byteLenCount;
            this.profitable = profitable;
            this.totalBitOptimal = totalBitOptimal;
            this.integerOptimal = integerOptimal;
            this.utfEightOptimal = utfEightOptimal;
        }
    }
}
