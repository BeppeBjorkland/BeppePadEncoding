namespace BitStringEncoding
{
    internal class ByteStructure
    {
        int maxByteLength = 22;
        int minByteLength = 1;
        int minPadding = 1;

        public void FindOptimalStructure(int targetValue)
        {
            Console.Clear();
            List<BestPick> bitOptimals = new List<BestPick>();
            BestPick bestPick = new BestPick(0, 0, ulong.MaxValue, ulong.MaxValue, new Dictionary<int, int>(), 0);
            BestPick secondBestPick = new BestPick(0, 0, ulong.MaxValue, ulong.MaxValue, new Dictionary<int, int>(), 0);
            Dictionary<int, int> byteLenCount = new Dictionary<int, int>();

            ulong maxCombinations = (ulong)Math.Pow(2, (int)Math.Ceiling(Math.Log2(targetValue)));
            //ulong staticCombinedBits = (ulong)Math.Pow(2, (int)Math.Ceiling(Math.Log2(targetValue))) * (ulong)(int)Math.Ceiling(Math.Log2(targetValue));
            ulong staticCombinedBits = maxCombinations * (ulong)(8 - (((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1) % 8) + ((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1));

            for (int i = minByteLength; i <= maxByteLength; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(i + " ");
                for (int j = minPadding; j <= i; j++)
                {
                    Console.SetCursorPosition(0, 1);
                    Console.WriteLine(j + " ");

                    byteLenCount = new Dictionary<int, int>();
                    ulong paddingCombinedBits = 0;
                    ulong validCombinations = 0;

                    string padding = "";
                    string binaryString = "0";
                    string topBinaryString = "";

                    for (int k = 0; k < j; k++)
                    {
                        padding += "0";
                    }
                    for (int k = 0; k < i; k++)
                    {
                        topBinaryString += "1";
                    }




                    do
                    {
                        int binaryValue = Convert.ToInt32(binaryString, 2);
                        binaryValue++;
                        binaryString = Convert.ToString(binaryValue, 2);

                        if (binaryValue % 10000 == 0)
                        {
                            Console.SetCursorPosition(0, 2);
                            Console.WriteLine(binaryValue + "               ");
                        }

                        //string tempBinaryString = binaryString + padding;


                        //if (tempBinaryString.IndexOf(padding) != -1 && tempBinaryString.IndexOf(padding) != tempBinaryString.LastIndexOf(padding))
                        if (!binaryString.Contains(padding))
                        {
                            validCombinations++;
                            paddingCombinedBits += (ulong)(j + binaryString.Length);

                            if (!byteLenCount.ContainsKey(j + binaryString.Length)) { byteLenCount.Add(j + binaryString.Length, 1); }
                            else { byteLenCount[j + binaryString.Length]++; }
                        }
                    } while (binaryString != topBinaryString);

                    int paramProfitable = 0;
                    foreach (var number in byteLenCount)
                    {
                        if (number.Key <= 8 - (((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1) % 8) + ((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1))
                        {
                            paramProfitable += number.Value;
                        }
                    }

                    if (validCombinations >= (ulong)targetValue && paddingCombinedBits < secondBestPick.paddingBits)
                    {
                        secondBestPick.paddingLen = j;
                        secondBestPick.byteLen = i;
                        secondBestPick.paddingBits = paddingCombinedBits;
                        secondBestPick.validCombinations = validCombinations;
                        secondBestPick.byteLenCount = byteLenCount;
                        secondBestPick.profitable = paramProfitable;

                        Console.SetCursorPosition(0, 4);
                        Console.WriteLine(
                            $"GOOD\n" +
                            $"Padding: {padding}\n" +
                            $"PaddingLen: {j}, binaryLen: {i}\n" +
                            $"Valid combinations {validCombinations}, Max combinations {maxCombinations}, Profitable: {paramProfitable}, Overshoot: {validCombinations - (ulong)targetValue}\n" +
                            $"Padding bits: {paddingCombinedBits}, Static bits {staticCombinedBits}, Static byte bits: {8 - (((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1) % 8) + ((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1)}\n");
                    }

                    if (paramProfitable >= targetValue && paddingCombinedBits < bestPick.paddingBits)
                    {
                        bestPick.paddingLen = j;
                        bestPick.byteLen = i;
                        bestPick.paddingBits = paddingCombinedBits;
                        bestPick.validCombinations = validCombinations;
                        bestPick.byteLenCount = byteLenCount;
                        bestPick.profitable = paramProfitable;

                        Console.SetCursorPosition(0, 4);
                        Console.WriteLine(
                            $"OPTIMAL\n" +
                            $"Padding: {padding}\n" +
                            $"PaddingLen: {j}, binaryLen: {i}\n" +
                            $"Valid combinations {validCombinations}, Max combinations {maxCombinations}, Profitable: {paramProfitable}, Overshoot: {validCombinations - (ulong)targetValue}\n" +
                            $"Padding bits: {paddingCombinedBits}, Static bits {staticCombinedBits}, Static byte bits: {8 - (((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1) % 8) + ((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1)}\n");
                    }
                }
            }

            Console.SetCursorPosition(0, 10);
            if (bestPick.paddingLen != 0)
            {
                Console.WriteLine(
                    $"Best pick\n" +
                    $"PaddingLen: {bestPick.paddingLen}, ByteLen: {bestPick.byteLen}\n" +
                    $"Valid numbers: {bestPick.validCombinations}\n" +
                    $"Bits: {bestPick.paddingBits}\n" +
                    $"Target: {targetValue}\n");

                int profitable = 0;

                foreach (var number in bestPick.byteLenCount)
                {
                    Console.WriteLine($"Bit: {number.Key}, Count: {number.Value}");
                    if (number.Key <= 8 - (((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1) % 8) + ((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1))
                    {
                        profitable += number.Value;
                    }
                }
                Console.WriteLine($"Profitable: {profitable} | {(float)profitable / bestPick.validCombinations * 100}%");
            }
            else
            {
                Console.WriteLine(
                    $"Second best pick\n" +
                    $"PaddingLen: {secondBestPick.paddingLen}, ByteLen: {secondBestPick.byteLen}\n" +
                    $"Valid numbers: {secondBestPick.validCombinations}\n" +
                    $"Bits: {secondBestPick.paddingBits}\n" +
                    $"Target: {targetValue}\n");

                int profitable = 0;

                foreach (var number in secondBestPick.byteLenCount)
                {
                    Console.WriteLine($"Bit: {number.Key}, Count: {number.Value}");
                    if (number.Key <= 8 - (((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1) % 8) + ((int)Math.Ceiling(Math.Log2(targetValue - 1)) - 1))
                    {
                        profitable += number.Value;
                    }
                }
                Console.WriteLine($"Profitable: {profitable} | {(float)profitable / secondBestPick.validCombinations * 100}%");
            }



        }
    }

    public class BestPick
    {
        public int paddingLen { get; set; }
        public int byteLen { get; set; }
        public int profitable { get; set; }
        public ulong paddingBits { get; set; }
        public ulong validCombinations { get; set; }
        public Dictionary<int, int> byteLenCount { get; set; }

        public BestPick(int paddingLen, int byteLen, ulong paddingBits, ulong validCombinations, Dictionary<int, int> byteLenCount, int profitable)
        {
            this.paddingLen = paddingLen;
            this.byteLen = byteLen;
            this.paddingBits = paddingBits;
            this.validCombinations = validCombinations;
            this.byteLenCount = byteLenCount;
            this.profitable = profitable;
        }
    }

}

