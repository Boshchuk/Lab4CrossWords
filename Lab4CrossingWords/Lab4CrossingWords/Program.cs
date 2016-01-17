using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4CrossingWords
{
    public class Word
    {
        public int Number { get; set; }
        public string Text { get; set; }

        public Word(int n, string text)
        {
            Number = n;
            Text = text;
        }
    }

    

    public class CrossPointInfo
    {
        public int Word1Number { get; set; }
        public int Word2Number { get; set; }

        public char Letter { get; set; }

        public int W1Pos { get; set; }
        public int W2Pos { get; set; }

        public bool IsSameWordsCross(CrossPointInfo toCheck)
        {
            return (Word1Number == toCheck.Word1Number && Word2Number == toCheck.Word2Number);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Word1Number, Word2Number );
        }
    }

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

        public static int DifferecntCross(List<CrossPointInfo> crossPointInfos)
        {
            var differecntCross = 0;
           

            foreach (var crossPointInfo in crossPointInfos)
            {
           //     crossPointInfo.
            }

            return differecntCross;
        }
    }
}
