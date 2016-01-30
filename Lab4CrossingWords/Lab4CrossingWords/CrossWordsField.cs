using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Lab4CrossingWords
{
    // We belave on tis unblimited size
    public class CrossWordsField
    {
        private int swapcount;

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

        public void InsertWord(CanPlace place, Word word)
        {
            if (place.CanTop)
            {
                InsertWord(word, PlaceDirection.Vertiacal, place.TopStart);
            }
            if (place.CanLeft)
            {
                InsertWord(word, PlaceDirection.Horisontal, place.LeftStart);
            }
        }

        public CanPlace CanPlaceToCrossPos(Cross crossPos, Word word)
        {
            Point shitTop = new Point(crossPos.Point.X, crossPos.Point.Y - word.Text.IndexOf(crossPos.Symbol));
            Point shitLeft = new Point(crossPos.Point.X- word.Text.IndexOf(crossPos.Symbol), crossPos.Point.Y );

            var canTop = true;
            var canLeft = true;

            var topCrossingCount = 0;
            var leftCrossingCount = 0;

            // try top
            for (int index = 0; index < word.Text.Length; index++)
            {
                var c = word.Text[index];
                var internalChar = InternalMatrix[shitTop.X , shitTop.Y + index];

                if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
                {
                    if (internalChar.Symbol == c)
                    {
                        topCrossingCount++;
                    }
                    if (topCrossingCount > 1)
                    {
                        canTop = false;
                        break;
                    }

                    continue;
                }
                else
                {
                    canTop = false;
                    break;
                }
            }

            for (int index = 0; index < word.Text.Length; index++)
            {
                var c = word.Text[index];
                var internalChar = InternalMatrix[shitLeft.X + index, shitLeft.Y];

                if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
                {
                    if (internalChar.Symbol == c)
                    {
                        leftCrossingCount++;
                    }
                    if (leftCrossingCount > 1)
                    {
                        canLeft = false;
                        break;
                    }
                    continue;
                }
                else
                {
                    canLeft = false;
                    break;
                }
            }
            return new CanPlace() {CanLeft = canLeft, CanTop = canTop, LeftStart = shitLeft,TopStart = shitTop};
        }

        public List<Cross> CrossPositions(Word word)
        {
            bool result = false;

            bool pointToInsertExist = false;

            var avalible = new List< Cross>();

            for (int i = 0; i < DimX; i++)
            {
                for (int j = 0; j < DimY; j++)
                {

                    foreach (var c in word.Text)
                    {
                        if (c == InternalMatrix[i, j].Symbol && InternalMatrix[i, j].PlacedCount <= 1)
                        {
                            pointToInsertExist = true;
                            avalible.Add(new Cross()
                            {
                                Point = new Point(i, j) ,
                                Symbol = c
                            });
                        }
                    }
                }
            }



            return avalible;
        }

        public bool ProcessWords2(List<Word> words)
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
                InsertWord(wordToPlace, PlaceDirection.Horisontal, new Point(DimX / 2, DimY / 2));

                words.Remove(wordToPlace);
                return ProcessWords2(words);
            }
            else
            {
                var canPlace = false;
                var next = words.FirstOrDefault();

                var avalibelePos = CrossPositions(next);

                if (avalibelePos.Count == 0)
                {
                    if (swapcount >=7)
                    {
                        return false;
                    }

                    words.Remove(next);
                    words.Add(next);
                    swapcount++;
                }
                else
                {
                  
                        // try insert
                        var result = false;


                            while (result == false)
                            {
                                var f = avalibelePos.FirstOrDefault();

                                if (f == null)
                                {
                                    break;
                                }

                                var cPos = CanPlaceToCrossPos(f, next);

                                if (cPos.Posible)
                                {
                                  InsertWord(cPos, next);

                                  words.Remove(next);

                                  result = true;
                                }
                                else
                                {
                                    avalibelePos.Remove(f);
                                    result = false;
                                }
                                
                            }

                        if (!result)
                        {
                            RemoveLastWord();
                            words.Remove(next);
                            words.Add(next);
                        }
                }

                return ProcessWords2(words);
            }

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