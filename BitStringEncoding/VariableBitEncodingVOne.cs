using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BitStringEncoding
{
    internal class VariableBitEncodingVOne
    {
        // SETTINGS
        string padding = "";
        int byteLen = 0;
        Dictionary<string, string> characterDictionary = new Dictionary<string, string>();


        public void SetSettings(int paddingLen, int byteLen)
        {
            DateTime settingStart = DateTime.Now;
            this.byteLen = byteLen;

            for (int i = 0; i < paddingLen; i++)
            {
                padding += "0";
            }
            Console.WriteLine($"Set settings time: {DateTime.Now - settingStart}");
        }

        public void SetDictionary (string input)
        {
            //List<string> characterList = new List<string>();
            //foreach (var character in input)
            //{
            //    if (!characterList.Contains(character.ToString()))
            //    {
            //        characterList.Add(character.ToString());
            //    }
            //}

            DateTime characterCountStart = DateTime.Now;
            Dictionary<string, int> characterCount = new Dictionary<string, int>();
            foreach (var character in input)
            {
                if (!characterCount.ContainsKey(character.ToString()))
                {
                    characterCount.Add(character.ToString(), 1);
                }
                else
                {
                    characterCount[character.ToString()]++;
                }
            }
            List<KeyValuePair<string, int>> tempList = characterCount.ToList();
            tempList.Sort((x, y) => x.Value - y.Value);
            //tempList.Sort((x, y) => y.Value - x.Value);
            List<string> characterList = tempList.Select((KeyValuePair<string, int> pair) => { return pair.Key; }).ToList();
            characterList.Reverse();

            Console.WriteLine($"Character frequensy order time: {DateTime.Now - characterCountStart}");

            DateTime setDictionaryTime = DateTime.Now;

            int index = 0;
            int binaryValue = 1;

            while (index < characterList.Count)
            {
                string binaryString = Convert.ToString(binaryValue, 2);
                binaryValue++;

                if (!binaryString.Contains(padding))
                {
                    

                    if (!characterDictionary.ContainsKey(characterList[index]))
                    {
                        List<bool> tempValue = (binaryString + padding).Select(c => c == '1').ToList();
                        characterDictionary.Add(characterList[index], string.Concat(tempValue.Select(b => b ? "1" : "0")));
                        Console.WriteLine(characterDictionary.Last().Key + " " + characterDictionary.Last().Value);
                        index++;
                    }

                    
                }
            }
            Console.WriteLine($"Set dictionary time: {DateTime.Now - setDictionaryTime}");

        }

        public List<bool> EncodeText(string input)
        {
            DateTime encodeTime = DateTime.Now;
            List<string> splitInput = input.Select(c => c.ToString()).ToList();
            List<bool> output = new List<bool>() { false, false};

            foreach (var character in splitInput)
            {
                //Console.Write(character);
                for (int i = 0; i < characterDictionary[character].Select(c => c == '1').ToList().Count; i++)
                {
                    output.Add(characterDictionary[character].Select(c => c == '1').ToList()[i]);
                }
            }

            //Console.WriteLine("\n" + string.Join("", output.Cast<bool>().Select(b => b ? "1" : "0")));
            Console.WriteLine($"Input characters: {input.Length}");

            Console.WriteLine($"Encode time: {DateTime.Now - encodeTime}");


            Dictionary<string, int> sequenceFrequency = new Dictionary<string, int>();
            string workString = string.Concat(output.Select(b => b ? "1" : "0"));
            char currentChar = workString[0];
            int sequenceCount = 1;
            for (int i = 1; i < workString.Length; i++)
            {
                if (workString[i] == currentChar)
                {
                    sequenceCount++;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(currentChar, sequenceCount);
                    if (!sequenceFrequency.ContainsKey(stringBuilder.ToString()))
                    {
                        sequenceFrequency.Add(stringBuilder.ToString(), 1);
                    }
                    else
                    {
                        sequenceFrequency[stringBuilder.ToString()]++;
                    }

                    currentChar = workString[i];
                    sequenceCount = 1;
                }
            }
            var sequenceFrequencyList = sequenceFrequency.ToList();
            sequenceFrequencyList.Sort((x, y) => x.Value - y.Value);

            foreach (var sequence in sequenceFrequencyList)
            {
                Console.WriteLine($"Sequence: {sequence.Key}, Frequency: {sequence.Value}");
            }


            return output;
        }

        public string DecodeText(List<bool> input)
        {
            DateTime decodeTime = DateTime.Now;
            var inverseDict = characterDictionary.ToDictionary(x => x.Value, x => x.Key);

            int characters = 0;
            int lastCharacterStart = input.Count;

            List<List<bool>> bitCharacterList = new List<List<bool>>();
            List<bool> paddingBits = new List<bool>(padding.Select(c => c == '1').ToList());

            for (int i = input.Count; i >= 0; i--)
            {
                int startIndex = Math.Max(i - padding.Length, 0);

                List<bool> inputCheck = new List<bool>(); // To Check if a padding is found
                for (int j = startIndex; j < i; j++)
                {
                    //Console.WriteLine($"i: {i}, j: {j}\ni-j: {i-j}");
                    //Console.WriteLine($"input len: {input.Count}");
                    inputCheck.Add(input[j]);
                    //Console.Write(input[j] + " ");
                }
                //Console.WriteLine();

                //foreach (var test in inputCheck)
                //{
                //    Console.Write(test + " ");
                //}
                //Console.WriteLine();
                //foreach (var test in paddingBits)
                //{
                //    Console.Write(test + " ");
                //}
                //Console.WriteLine($"\ninput len: {inputCheck.Count}, padding len: {paddingBits.Count}");

                bool isEqual = false;
                if (inputCheck.Count == paddingBits.Count)
                {
                    isEqual = true;
                    for (int j = 0; j < inputCheck.Count; j++)
                    {
                        if (inputCheck[j] != paddingBits[j])
                        {
                            isEqual = false;
                            break;
                        }
                    }
                }
                if (isEqual)
                {
                    characters++;
                    i -= padding.Length - 1;
                    //Console.WriteLine($"Characters: {characters}");

                    if (characters > 1)
                    {
                        //Console.WriteLine($"Lastchar: {lastCharacterStart}, this char: {i}");
                        List<bool> newCharacter = new List<bool>();
                        for (int j = 0; j < lastCharacterStart - i; j++)
                        {
                            //Console.WriteLine($"i; {i}, j: {j}, max: {input.Count - 1}, this: {i + j + 1}, j max: {lastCharacterStart - i}");
                            //Console.WriteLine("Input count: " + input.Count);
                            //Console.WriteLine("Index tried: " + (i + j + 2));
                            newCharacter.Add(input[i + j + 1]);
                            //Console.Write(input[i + j + 2] + " ");
                        }
                        //Console.WriteLine();
                        bitCharacterList.Add(newCharacter);
                    }

                    lastCharacterStart = i;
                }


            }
            string output = "";
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var character in bitCharacterList)
            {
                //Console.WriteLine(string.Join("", character.Cast<bool>().Select(b => b ? "1" : "0")));
                //Console.WriteLine(characterDictionary.FirstOrDefault(x => x.Value == character).Key);
                //output += characterDictionary.FirstOrDefault(x => x.Value == character).Key;

                if (characterDictionary.ContainsValue(string.Concat(character.Select(b => b ? "1" : "0"))))
                {
                    //Console.WriteLine(string.Join("", character.Cast<bool>().Select(b => b ? "1" : "0")));
                    //Console.WriteLine(characterDictionary.FirstOrDefault(x => x.Value == string.Concat(character.Select(b => b ? "1" : "0"))).Key);
                    //output += characterDictionary.FirstOrDefault(x => x.Value == string.Concat(character.Select(b => b ? "1" : "0"))).Key;
                    stringBuilder.Append(characterDictionary.FirstOrDefault(x => x.Value == string.Concat(character.Select(b => b ? "1" : "0"))).Key);
                }
            }
            output = ReverseString(stringBuilder.ToString());
            //Console.WriteLine(output);
            Console.WriteLine($"Encoded bits: {input.Count}, UTF-8 bits: {Encoding.UTF8.GetByteCount(output) * 8}");
            Console.WriteLine($"Encoded percentage: {input.Count / (float)(Encoding.UTF8.GetByteCount(output) * 8) * 100}%");

            Console.WriteLine($"Decode time: {DateTime.Now - decodeTime}");

            return output;
        }

        public string ReverseString(string stringToReverse)
        {
            var charArray = stringToReverse.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public string RunLengthEncoder(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (input[0] == '0' && input[1] == '0') { stringBuilder.Append('0'); }
            else { stringBuilder.Append('1'); }


            int zeroSequence = 0;
            for (int i = 1; i < input.Length; i += 2)
            {
                bool triggered = false;
                if (input[i-1] == '0' && input[i] == '0')
                {
                    zeroSequence++;
                    triggered = true;
                }
                else
                {
                    stringBuilder.Append(input[i - 1]);
                    stringBuilder.Append(input[i]);
                    if (i+2 <= input.Length && input[i+1] == '0' && input[i + 2] == '0')
                    {
                        stringBuilder.Append("00");
                    }
                }

                if (zeroSequence > 0 && (!triggered || i == input.Length - 1))
                {
                    string sequenceBits = Convert.ToString(zeroSequence + 1, 2);
                    string V = sequenceBits.Substring(1);
                    int bitNumber = Convert.ToInt32(sequenceBits, 2);
                    string L = Convert.ToString((zeroSequence + 1) - Convert.ToInt32(V, 2) - 2, 2);
                    //Console.WriteLine("Zero Sequence: " + (zeroSequence));
                    //Console.WriteLine("V: " + sequenceBits.Substring(1));
                    //Console.WriteLine("L: " + L);
                    //Console.WriteLine("Original: " + (Convert.ToInt32(V, 2) + Convert.ToInt32(L, 2) + 1));

                    stringBuilder.Append(L + V);
                    zeroSequence = 0;
                }
            }
            return stringBuilder.ToString();
        }

        public string DeltaCoding(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            char lastChar = input[0];
            stringBuilder.Append(lastChar);

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] == lastChar)
                {
                    stringBuilder.Append('0');
                }
                else
                {
                    stringBuilder.Append('1');
                }
                lastChar = input[i];
            }


            return stringBuilder.ToString();
        }
    }
}
