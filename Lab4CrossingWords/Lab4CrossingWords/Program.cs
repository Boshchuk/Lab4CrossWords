﻿using System;
using System.Collections.Generic;

namespace Lab4CrossingWords
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordsList = new List<Word>
            {
                new Word(1, "художник"),
                new Word(2, "кисть"),
                new Word(3, "мастер"),
                new Word(4, "студия"),
                new Word(5, "пейзаж"),
                new Word(6, "палитра"),
                new Word(7, "мольберт")
            };


            List<CrossPointInfo> crossPointInfos = GetCrossPointInfos(wordsList);

            foreach (var crossPointInfo in crossPointInfos)
            {
                Console.WriteLine(crossPointInfo.ToString()); 
            }

            Console.ForegroundColor = ConsoleColor.Blue;

            List<CrossPointInfo> differecntCross = DifferentCross(crossPointInfos);

            foreach (var crossPointInfo in differecntCross)
            {
                Console.WriteLine(crossPointInfo.ToString());
            }
            Console.ForegroundColor = ConsoleColor.Green;



            Console.WriteLine(differecntCross.Count);

            if (differecntCross.Count < 7 - 1)
            {
                Console.WriteLine("Can't build");
            }
            else
            {
                // go to deeper check
            }

        

            Console.Read();
        }

        public static List<CrossPointInfo> GetCrossPointInfos(List<Word> words)
        {
            var result = new List<CrossPointInfo>();

            for (int index = 0; index < words.Count -1; index++)
            {
                for (int seconwIndex = index +1; seconwIndex < words.Count; seconwIndex++)
                {
                    result.AddRange(GetCrossInfo(words[index], words[seconwIndex]));
                }
            }

            return result;
        }

        public static List<CrossPointInfo> GetCrossInfo(Word word1, Word word2)
        {
            var result = new List<CrossPointInfo>();
            for (int i = 0; i < word1.Text.Length; i++)
            {
                var w1 = word1.Text[i];
                for (int index = 0; index < word2.Text.Length; index++)
                {
                    char w2 = word2.Text[index];
                    if (w1 == w2)
                    {
                        var info = new CrossPointInfo()
                        {
                            Letter = w1,
                            W1Pos = i,
                            W2Pos = index,
                            Word1Number = word1.Number,
                            Word2Number = word2.Number,
                        };
                        result.Add(info);
                    }
                }
            }
            return result;
        }

        public static List<CrossPointInfo> DifferentCross(List<CrossPointInfo> crossPointInfos)
        {
            var differecntCross = 0;

            List < CrossPointInfo > unicCrossPointInfos = new List<CrossPointInfo>();
            foreach (var crossPointInfo in crossPointInfos)
            {
                
                if (unicCrossPointInfos.Count == 0)
                {
                    unicCrossPointInfos.Add(crossPointInfo);
                }
                var needAdd = true;
                foreach (var unicCrossPointInfo in unicCrossPointInfos)
                {
                    if (crossPointInfo.IsSameWordsCross(unicCrossPointInfo))
                    {
                        needAdd = false;
                        break;
                    }
                }

                if (needAdd)
                {
                    unicCrossPointInfos.Add(crossPointInfo);
                }
                
            }

            return unicCrossPointInfos;
        }
    }
}
