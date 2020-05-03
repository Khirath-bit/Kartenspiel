
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

        public string ImgSrc { get; set; }
    }
}
