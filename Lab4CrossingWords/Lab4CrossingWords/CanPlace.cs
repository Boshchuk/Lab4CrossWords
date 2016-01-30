using System.Drawing;

namespace Lab4CrossingWords
{
    public class CanPlace
    {
        public bool CanTop { get; set; }

        public Point TopStart { get; set; }


        public bool CanLeft { get; set; }

        public Point LeftStart { get; set; }

        public bool Posible
        {
            get { return (CanLeft || CanTop); }
        }
    }
}