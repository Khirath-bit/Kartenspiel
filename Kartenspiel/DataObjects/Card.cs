
using System.IO;

namespace Kartenspiel.DataObjects
{
    public class Card
    {
        public Enums.CardSigns CardSigns { get; set; }

        public string Description { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            return CardSigns + " " + Description;
        }

        public string Img { get; set; }

        public string ImgSrc 
        { 
            get
            {
                if (Show)
                    return Img;
                else
                    return Back;
            } 
        }

        public string Back { get; set; } = Path.GetFullPath("CardImages/Back.jpg");

        public bool Show { get; set; } = false;


    }
}
