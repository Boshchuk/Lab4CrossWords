using System;

namespace Lab4CrossingWords
{
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
            return $"{Word1Number} {Word2Number}";
        }


        public int GetThisCross(int firstKey)
        {
            if (firstKey == Word1Number)
            {
                return W1Pos;
            }

            if (firstKey == Word2Number)
            {
                return W2Pos;
            }

            throw new ArgumentException();
        }


        public int GetPairedCrossNum(int oneOfPairNum)
        {
            if (oneOfPairNum == Word1Number)
            {
                return Word2Number;
            }

            if (oneOfPairNum == Word2Number)
            {
                return Word1Number;
            }

            throw new ArgumentException();
        }

    }
}