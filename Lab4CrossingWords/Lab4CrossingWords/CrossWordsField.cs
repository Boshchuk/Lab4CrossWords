using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace Lab4CrossingWords
{
    // We belave on tis unblimited size
    public class CrossWordsField
    {
        private readonly int _wordsToInsert;
        private const char DefSymbol = '*';

        private int DimX { get; set; }
        private int DimY { get; set; }

        private InternalChar[,] InternalMatrix { get; set; }

        private Stack<PlacedWord> PlacedWords { get; set; }

        public CrossWordsField(int dimX, int dimY, int wordsToInsert)
        {
            PlacedWords = new Stack<PlacedWord>();
            _wordsToInsert = wordsToInsert;
            DimX = dimX;
            DimY = dimY;
            InternalMatrix = new InternalChar[DimX, DimY];
            for (int i = 0; i < DimX; i++)
            {
                for (int j = 0; j < DimY; j++)
                {
                    InternalMatrix[i, j] = new InternalChar
                    {
                        PlacedCount = 0,
                        Symbol = DefSymbol
                    };
                }
            }
        }




        public bool CanPlaceWord(PlacedWord placedWord, CrossPointInfo info, PlaceDirection direction, Word word)
        {

            // var 


            Point pointToPlace = new Point();

            Point crossPoint = new Point();

            switch (placedWord.PlaceDirection)
            {
                case PlaceDirection.Horisontal:
                    {
                        crossPoint.X = placedWord.StartPoint.X + info.GetThisCross(placedWord.Word.Number);
                        crossPoint.Y = placedWord.StartPoint.Y;
                        break;
                    }
                case PlaceDirection.Vertiacal:
                    {
                        crossPoint.X = placedWord.StartPoint.X;
                        crossPoint.Y = placedWord.StartPoint.Y + info.GetThisCross(placedWord.Word.Number);
                        break;
                    }
            }

            switch (direction)
            {
                case PlaceDirection.Horisontal:
                    {
                        pointToPlace.X = crossPoint.X - info.GetThisCross(word.Number);
                        pointToPlace.Y = crossPoint.Y;
                        break;
                    }
                case PlaceDirection.Vertiacal:
                    {
                        pointToPlace.X = crossPoint.X;
                        pointToPlace.Y = crossPoint.Y - info.GetThisCross(word.Number);
                        break;
                    }
            }

            var result = true;
            switch (direction)
            {
                case PlaceDirection.Horisontal:
                    {
                        for (int index = 0; index < word.Text.Length; index++)
                        {
                            var c = word.Text[index];
                            var internalChar = InternalMatrix[pointToPlace.X + index, pointToPlace.Y];

                            if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
                            {
                                continue;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        break;
                    }

                case PlaceDirection.Vertiacal:
                    {
                        for (int index = 0; index < word.Text.Length; index++)
                        {
                            var c = word.Text[index];
                            var internalChar = InternalMatrix[pointToPlace.X, pointToPlace.Y + index];

                            if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
                            {
                                continue;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        break;

                    }
            }




            return result;
        }

        public void ProcessWords(List<Word> words, Dictionary<int, List<CrossPointInfo>> dictionary)
        {
            if (words.Count == 0)
            {
                Console.WriteLine("all placed");
            }

            var wordToPlace = words.First();

            if (PlacedWords.Count == 0)
            {
                InsertWord(wordToPlace, PlaceDirection.Horisontal, null);

                var minusOne = words.Remove(wordToPlace);
                ProcessWords(words, dictionary);
            }
            else
            {
                var relatedInfos = dictionary[wordToPlace.Number];

                CrossPointInfo attemptPlace = relatedInfos.First(x => x.Word1Number == wordToPlace.Number || x.Word2Number == wordToPlace.Number);

                var idOfWordToConnect = attemptPlace.GetPairedCrossNum(wordToPlace.Number);

                PlacedWord placedToConnect = PlacedWords.First(x => x.Word.Number == idOfWordToConnect);

                PlaceDirection dir = PlaceDirection.Horisontal;

                switch (placedToConnect.PlaceDirection)
                {
                    case PlaceDirection.Horisontal:
                        {
                            dir = PlaceDirection.Vertiacal;
                            break;
                        }
                    case PlaceDirection.Vertiacal:
                        {
                            dir = PlaceDirection.Horisontal;
                            break;
                        }
                }

                if (CanPlaceWord(placedToConnect, attemptPlace, dir, wordToPlace))
                {
                    InsertWord(wordToPlace, dir, attemptPlace);

                    var minusOne = words.Remove(wordToPlace);
                    
                }
                else
                {
                    dictionary[wordToPlace.Number].Remove(attemptPlace);
                    words.Remove(wordToPlace);
                    words.Add(wordToPlace);
                }
                ProcessWords(words, dictionary);
            }

        }

        private void InsertWordHorisontal(Word word, Point point)
        {
            for (int index = 0; index < word.Text.Length; index++)
            {
                var c = word.Text[index];
                InternalMatrix[point.X + index, point.Y].Symbol = c;
                InternalMatrix[point.X + index, point.Y].PlacedCount++;
            }
        }

        private void InsertWordVertical(Word word, Point point)
        {
            for (int index = 0; index < word.Text.Length; index++)
            {
                var c = word.Text[index];
                InternalMatrix[point.X, point.Y + index].Symbol = c;
                InternalMatrix[point.X, point.Y + index].PlacedCount++;
            }
        }

        public void InsertWord(Word word, PlaceDirection direction, CrossPointInfo crossPointInfo)
        {
            var placedWord = new PlacedWord
            {
                Word = word
            };

            if (PlacedWords.Count == 0)
            {
                // inserting the first-first
                placedWord.StartPoint = new Point(50, 50);
                placedWord.PlaceDirection = PlaceDirection.Horisontal;
            }
            else
            {
                placedWord.PlaceDirection = direction;
            }


            // TODO: determenate Direction
            switch (placedWord.PlaceDirection)
            {

                case PlaceDirection.Horisontal:
                    {
                        InsertWordHorisontal(word, placedWord.StartPoint);
                        break;
                    }
                case PlaceDirection.Vertiacal:
                    {
                        InsertWordVertical(word, placedWord.StartPoint);
                        break;
                    }
            }

            PlacedWords.Push(placedWord);
        }

        public void RemoveLastWord()
        {
            var placedWord = PlacedWords.Pop();

            switch (placedWord.PlaceDirection)
            {
                case PlaceDirection.Horisontal:
                    {
                        for (var index = 0; index < placedWord.Word.Text.Length; index++)
                        {
                            InternalMatrix[placedWord.StartPoint.X + index, placedWord.StartPoint.Y].PlacedCount--;

                            if (InternalMatrix[placedWord.StartPoint.X + index, placedWord.StartPoint.Y].PlacedCount == 0)
                            {
                                InternalMatrix[placedWord.StartPoint.X + index, placedWord.StartPoint.Y].Symbol = DefSymbol;
                            }
                        }

                        break;
                    }
                case PlaceDirection.Vertiacal:
                    {
                        for (var index = 0; index < placedWord.Word.Text.Length; index++)
                        {
                            InternalMatrix[placedWord.StartPoint.X, placedWord.StartPoint.Y + index].PlacedCount--;

                            if (InternalMatrix[placedWord.StartPoint.X, placedWord.StartPoint.Y + index].PlacedCount == 0)
                            {
                                InternalMatrix[placedWord.StartPoint.X, placedWord.StartPoint.Y + index].Symbol = DefSymbol;
                            }
                        }

                        break;
                    }
            }
        }
    }
}