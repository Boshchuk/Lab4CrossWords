using System.Collections.Generic;
using System.Drawing;

namespace Lab4CrossingWords
{
    // We belave on tis unblimited size
    public class CrossWordsField
    {
        private readonly int _wordsToInsert;
        private const char DefSymbol = '*';

        public int DimX { get; set; }
        public int DimY { get; set; }

        private char[,] Internal { get; set; }

        private List<PlacedWord> PlacedWords { get; set; }

        public CrossWordsField(int dimX, int dimY, int wordsToInsert)
        {
            _wordsToInsert = wordsToInsert;
            DimX = dimX;
            DimY = dimY;
            Internal = new char[DimX, DimY];
            for (int i = 0; i < DimX; i++)
            {
                for (int j = 0; j < DimY; j++)
                {
                    Internal[i, j] = DefSymbol;
                }
            }
        }

        public bool CanPalceWord(Point pointToPlace, PlaceDirection direction, Word word)
        {
            var result = true;
            switch (direction)
            {
                case PlaceDirection.Horisontal:
                {
                       for (int index = 0; index < word.Text.Length; index++)
                        {
                            var c = word.Text[index];
                            var internalChar = Internal[pointToPlace.X + index, pointToPlace.Y];

                            if ((internalChar == DefSymbol) || (internalChar == c))
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
                            var internalChar = Internal[pointToPlace.X, pointToPlace.Y + index];

                            if ((internalChar == DefSymbol) || (internalChar == c))
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

        public void ProcessWords(List<Word> words,)
        {
            while (PlacedWords.Count != _wordsToInsert)
            {
                
            }
        }


        public void InsertWord(Word word)
        {
            if (PlacedWords.Count == 0)
            {
                // if first inset in mid at horisontal
            }
        }
    }
}