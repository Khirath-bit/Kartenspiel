using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kartenspiel.DataObjects;
using Kartenspiel.Games;

namespace Kartenspiel.Manager
{
    public static class GameManager
    {
        private static CardManager _cardManager;

        private static PlayerManager _playerManager;

        private static IGame _currentGame;

        public static void StartNewGame(Enums.Games game, string playerName)
        {
            _cardManager = new CardManager(game);
            _playerManager = new PlayerManager();
            //_playerManager.GenerateAiPlayers(3);
            _playerManager.GenerateHumanPlayer(playerName);

            switch (game)
            {
                case Enums.Games.Blackjack:
                    _currentGame = new Blackjack(_playerManager, _cardManager);
                    break;
                default:
                    _currentGame = new Blackjack(_playerManager, _cardManager);
                    break;
            }
        }
    }
}
