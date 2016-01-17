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
}