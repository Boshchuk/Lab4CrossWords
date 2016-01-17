namespace Lab4CrossingWords
{
    public class CrossWordsField
    {
        public int DimX { get; set; }
        public int DimY { get; set; }

        private char[,] Internal { get; set; }

        public CrossWordsField(int dimX, int dimY, char defSymbol)
        {
            DimX = dimX;
            DimY = dimY;
            Internal = new char[DimX, DimY];
            for (int i = 0; i < DimX; i++)
            {
                for (int j = 0; j < DimY; j++)
                {
                    Internal[i, j] = '*';
                }
            }
        }


    }
}