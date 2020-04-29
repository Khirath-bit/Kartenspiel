using Kartenspiel.DataObjects;
using System.Collections.Generic;

namespace Kartenspiel.Manager
{
    public class CardManager
    {

        private List<string> _names = new List<string>()
        {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "Bube",
            "Dame",
            "König",
            "Ass"
        };

        public List<Card> Cards { get; set; }

        public CardManager(Enums.Games cardAmount)
        {
            GenerateCards(cardAmount);
        }

        private void GenerateCards(Enums.Games cardAmount)
        { 
            Cards = new List<Card>();

            var count = (int)cardAmount / 4;

            for (var i = 0; i < count; i++)
            {
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Herz, Description = _names[i], Value = i + 2 });
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Karo, Description = _names[i], Value = i + 2 });
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Kreuz, Description = _names[i], Value = i + 2 });
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Pik, Description = _names[i], Value = i + 2 });
            }

            Cards.Shuffle();
        }
    }
}
