using Kartenspiel.DataObjects;
using System.Collections.Generic;
using System.IO;

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
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Herz, Description = _names[i], Value = i + 2, ImgSrc = "CardImages/" + (int.TryParse(_names[i], out int v) ? v + "H.jpg" : _names[i][0] + "H.jpg")});
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Karo, Description = _names[i], Value = i + 2, ImgSrc = "CardImages/" + (int.TryParse(_names[i], out int v2) ? v2 + "D.jpg" : _names[i][0] + "D.jpg" )});
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Kreuz, Description = _names[i], Value = i + 2, ImgSrc = "CardImages/" + (int.TryParse(_names[i], out int v3) ? v3 + "C.jpg" : _names[i][0] + "C.jpg" )});
                Cards.Add(new Card { CardSigns = Enums.CardSigns.Pik, Description = _names[i], Value = i + 2, ImgSrc = "CardImages/" + (int.TryParse(_names[i], out int v4) ? v4 + "S.jpg" : _names[i][0] + "S.jpg" )});
            }

            Cards.ForEach(p => 
            p.ImgSrc = Path.GetFullPath(p.ImgSrc)
            );

            Cards.Shuffle();
        }
    }
}
