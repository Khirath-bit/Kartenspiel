using Kartenspiel.Manager;
using System.Collections.Generic;

namespace Kartenspiel.DataObjects
{
    public class Player
    {
        public string Name { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();

        public int Augenzahl 
        { 
            get
            {
                int v = 0;
                foreach (var item in Cards)
                    v += item.Value;
                return v;
            } 
        }
    }
}
