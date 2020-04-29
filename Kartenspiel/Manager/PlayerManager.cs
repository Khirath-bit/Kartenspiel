using System.Collections.Generic;
using Kartenspiel.DataObjects;

namespace Kartenspiel.Manager
{
    public class PlayerManager
    {
        public List<Player> AiPlayers { get; set; }

        public Player Player { get; set; }

        public void GenerateAiPlayers(int amount)
        {
            AiPlayers = new List<Player>();

            for (int i = 0; i < amount; i++)
            {
                AiPlayers.Add(new Player{Name = "RandomBot " + AiPlayers.Count+1});
            }
        }

        public void GenerateHumanPlayer(string name)
        {
            Player = new Player{Name = name};
        }
    }
}
