using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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

        public void ClearInternal()
        {
            PlacedWords.Clear();

            for (var i = 0; i < DimX; i++)
            {
                for (var j = 0; j < DimY; j++)
                {
                    InternalMatrix[i, j].Symbol = DefSymbol;
                    InternalMatrix[i, j].PlacedCount = 0;
                }
            }
        }

        public void DrawMatrix()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < DimX; i++)
            {
                
                for (int j = 0; j < DimY; j++)
                {
                    var internalChar = InternalMatrix[i, j];

                    if (internalChar.Symbol != DefSymbol)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        
                    }
                    if (internalChar.PlacedCount > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write("{0}",internalChar.Symbol );
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.WriteLine();
            }
        }

        public Point CulcStartPoint(PlacedWord placedWord, CrossPointInfo info, PlaceDirection direction, Word word)
        {
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
            return pointToPlace;
        }

        public bool CanPlaceWord(PlacedWord placedWord, CrossPointInfo info, PlaceDirection direction, Word word)
        {

            // var 
            var pointToPlace = CulcStartPoint(placedWord, info, direction, word);


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

        public bool ProcessWords(List<Word> words, Dictionary<int, List<CrossPointInfo>> dictionary)
        {
            if (words.Count == 0)
            {
                if (PlacedWords.Count == _wordsToInsert)
                {
                    Console.WriteLine("all placed");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            var wordToPlace = words.First();

            if (PlacedWords.Count == 0)
            {
                InsertWord(wordToPlace, PlaceDirection.Horisontal, new Point(DimX/2,DimY/2));

                var minusOne = words.Remove(wordToPlace);
                return ProcessWords(words, dictionary);
            }
            else
            {
                var canPlace = false;

                var relatedInfos = dictionary[wordToPlace.Number];

                CrossPointInfo attemptPlace = relatedInfos.FirstOrDefault(x => x.Word1Number == wordToPlace.Number || x.Word2Number == wordToPlace.Number);

                if (attemptPlace != null)
                {
                    var idOfWordToConnect = attemptPlace.GetPairedCrossNum(wordToPlace.Number);

                    PlacedWord placedToConnect = PlacedWords.FirstOrDefault(x => x.Word.Number == idOfWordToConnect);

                    if (placedToConnect != null)
                    {
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
                            canPlace = true;
                            var startPoint = CulcStartPoint(placedToConnect, attemptPlace, dir, wordToPlace);
                            InsertWord(wordToPlace, dir, startPoint);

                            var minusOne = words.Remove(wordToPlace);
                        }
                    }

                }
                else
                {
                    return false;
                }

                if (!canPlace)
                {
                    dictionary[wordToPlace.Number].Remove(attemptPlace);
                    words.Remove(wordToPlace);
                    words.Add(wordToPlace);
                }
                return ProcessWords(words, dictionary);
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

        public void InsertWord(Word word, PlaceDirection direction, Point startPoint)
        {
            var item = new PlacedWord
            {
                Word = word,
                StartPoint = startPoint,
                PlaceDirection = direction
            };

            switch (item.PlaceDirection)
            {

                case PlaceDirection.Horisontal:
                    {
                        InsertWordHorisontal(word, item.StartPoint);
                        break;
                    }
                case PlaceDirection.Vertiacal:
                    {
                        InsertWordVertical(word, item.StartPoint);
                        break;
                    }
            }

            PlacedWords.Push(item);
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