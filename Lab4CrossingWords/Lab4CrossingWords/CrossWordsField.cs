using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

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

        public bool CanPlaceWord(Point pointToPlace, PlaceDirection direction, Word word)
        {
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

        public void ProcessWords(List<Word> words, List<CrossPointInfo> infos)
        {
            while (PlacedWords.Count != _wordsToInsert)
            {
                
            }
        }

        private void InsertWordHorisontal(Word word, Point point)
        {
            for (int i = 0; i < DimX; i++)
            {
                for (int j = 0; j < DimY; j++)
                {
                    for (int index = 0; index < word.Text.Length; index++)
                    {
                        var c = word.Text[index];
                        InternalMatrix[point.X + index, point.Y].Symbol = c;
                        InternalMatrix[point.X + index, point.Y].PlacedCount++;
                    }
                }
            }
        }

        private void InsertWordVertical(Word word, Point point)
        {
            for (int i = 0; i < DimX; i++)
            {
                for (int j = 0; j < DimY; j++)
                {
                    for (int index = 0; index < word.Text.Length; index++)
                    {
                        var c = word.Text[index];
                        InternalMatrix[point.X, point.Y +index].Symbol = c;
                        InternalMatrix[point.X, point.Y +index].PlacedCount++;
                    }
                }
            }
        }

        public void InsertWord(Word word, List<CrossPointInfo> relatedInfo)
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
                //todo insert second
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

        public void RemoveWord(Word word)
        {
            
        }
    }
}