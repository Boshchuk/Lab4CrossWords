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
            return string.Format("{0} {1}", Word1Number, Word2Number);
        }
    }
}