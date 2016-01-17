using System;
using System.Collections.Generic;

namespace Lab4CrossingWords
{
    class Program
    {
        static void Main(string[] args)
        {
            var wordsList = new List<Word>();
            wordsList.Add (new Word(1, "художник"));
            wordsList.Add(new Word(2, "кисть"));
            wordsList.Add(new Word(3, "мастер"));
            wordsList.Add(new Word(4, "студия"));
            wordsList.Add(new Word(5, "пейзаж"));
            wordsList.Add(new Word(6, "палитра"));
            wordsList.Add(new Word(7, "мольберт"));


            List<CrossPointInfo> res = GetCrossPointInfos(wordsList);

            foreach (var crossPointInfo in res)
            {
                Console.WriteLine(crossPointInfo.ToString()); 
            }

            Console.ForegroundColor = ConsoleColor.Blue;

            var r2 = DifferecntCross(res);

            foreach (var crossPointInfo in r2)
            {
                Console.WriteLine(crossPointInfo.ToString());
            }
            Console.ForegroundColor = ConsoleColor.Green;



            Console.WriteLine(r2.Count);

            if (r2.Count < 7 - 1)
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

        public static List<CrossPointInfo> DifferecntCross(List<CrossPointInfo> crossPointInfos)
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
