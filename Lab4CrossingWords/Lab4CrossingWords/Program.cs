using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab4CrossingWords
{
    class Program
    {
        static int count  {get; set; }

        static Dictionary<int, List<Word>> combinations = new Dictionary<int, List<Word>>();

        static void Main(string[] args)
        {
            var wordsList = new List<Word>
            {
                //new Word(1, "художник"),
                //new Word(2, "кисть"),
                //new Word(3, "мастер"),
                //new Word(4, "студия"),
                //new Word(5, "пейзаж"),
                //new Word(6, "палитра"),
                //new Word(7, "мольберт")

                //new Word(1, "мольберт"),
                //new Word(2, "мeстер"),
                //new Word(3, "палитра"),
                //new Word(4, "сту"),
                //new Word(5, "худ"),
                //new Word(6, "пейзаж"),
                //new Word(7, "кис"),

                new Word(1, "мольберт"),
                new Word(2, "мeстер"),
                new Word(3, "палитра"),
                new Word(4, "сту"),
                new Word(5, "худ"),
                new Word(6, "пейзаж"),
                new Word(7, "кис"),

            };

            Perestanovki(wordsList, 7);

            


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(count);

            // raw cross posint infos
            List<CrossPointInfo> crossPointInfos = GetCrossPointInfos(wordsList);

            foreach (var crossPointInfo in crossPointInfos)
            {
                Console.WriteLine(crossPointInfo.ToString()); 
            }
            // end


            // for words avalible crossong calc
            var nList = wordsList.Select(x => x.Number);

            Dictionary<int, List<CrossPointInfo>> dick = new Dictionary<int, List<CrossPointInfo>>();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;

            foreach (var key in nList)
            {
                var res = GetRelatedForWord(key, crossPointInfos);


                dick.Add(key, res);


                foreach (var crossPointInfo in res)
                {
                    Console.WriteLine(crossPointInfo.ToString());
                }

                Console.WriteLine();
            }


            // end calc and display
            

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

                CrossWordsField field = new CrossWordsField(50,50, 7);

                var words = new List<Word>(wordsList);
                var d = new Dictionary<int, List<CrossPointInfo>>(dick);


                var res = false;
                var attemts = count-1;
                while (!res && attemts !=0)
                {
                    res = field.ProcessWords(words, d);


                    if (res == false)
                    {
                        

                        //var first = wordsList.First();
                        //wordsList.Remove(first);
                        //wordsList.Add(first);
                        //words = new List<Word>(wordsList);

                        words = combinations[attemts];

                        d = new Dictionary<int, List<CrossPointInfo>>(dick);

                        field.ClearInternal();
                        attemts--;
                    }

                }

                if (attemts == 0 && res == false)
                {
                    Console.WriteLine("Низя");
                }
                else
                {
                    field.DrawMatrix();
                }
                    
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

        public static List<CrossPointInfo> GetRelatedForWord(int wordKey, List<CrossPointInfo> crossPointInfos)
        {
            var differecntCross = 0;

            List<CrossPointInfo> infos = new List<CrossPointInfo>();
            foreach (var crossPointInfo in crossPointInfos.Where(x=>x.Word1Number == wordKey || x.Word2Number == wordKey ))
            {

                if (infos.Count == 0)
                {
                    infos.Add(crossPointInfo);
                }
                var needAdd = true;
                foreach (var unicCrossPointInfo in infos)
                {
                    if (crossPointInfo.IsSameWordsCross(unicCrossPointInfo))
                    {
                        needAdd = false;
                        break;
                    }
                }

                if (needAdd)
                {
                    infos.Add(crossPointInfo);
                }

            }

            return infos;
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

     

        public static List<Word> Swap(int keya, int keyb, List<Word> words)
        {
            var t = new Word(words.FirstOrDefault(x => x.Number == keya).Number, words.FirstOrDefault(x => x.Number == keya).Text) ;
            Word b = words.FirstOrDefault(x => x.Number == keyb);


            var a = words.FirstOrDefault(x => x.Number == keya);


            


            a.Text = b.Text;
            a.Number = b.Number;



            b.Text = t.Text;
            b.Number = t.Number;
            

            return words;
        }

        
        static void Obrabotka(List<Word> words )
        {
            combinations.Add(count, new List<Word>(words));

            //foreach (var variable in words)
            //{
            //    Console.Write("{0}", variable.Number);
            //}
            count++;
           // Console.WriteLine();

        }
        
        static void Perestanovki(List<Word> words, int n)//Words - массив, n - число переставляемых элементов, 
                                                         //N - реальный размер массива
        {
            if (n == 1)
                Obrabotka(words); //если нечего переставлять
            else
            {
                for (int i = 0; i < n; i++)
                {
                    words = Swap(words[i].Number, words[n - 1].Number, words); //меняем последний элемент с каждым,
                                          //в том числе и с самим собой.
                    Perestanovki(words, n - 1); //запускаем функцию, для n-1 элементов
                    words = Swap(words[i].Number, words[n - 1].Number, words); //поигрались - и хватит. Надо вернуть массив в прежнее
                                          //состояние для следующего обмена элементов
                }
            }
        }
    }
}
