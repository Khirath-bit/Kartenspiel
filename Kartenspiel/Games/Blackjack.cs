using System;
using System.Collections.Generic;
using Kartenspiel.DataObjects;
using Kartenspiel.Manager;

namespace Kartenspiel.Games
{
    public class Blackjack : IGame
    {
        public Blackjack(PlayerManager playerManager, CardManager cardManager)
        {
            PlayerManager = playerManager;
            CardManager = cardManager;
        }

        public PlayerManager PlayerManager { get; set; }

        public CardManager CardManager { get; set; }

        public void PlayGame()
        {
            throw new NotImplementedException();
        }
    }
}
