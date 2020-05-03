using Kartenspiel.DataObjects;
using Kartenspiel.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MVVM;

namespace Kartenspiel.Games
{
    public class BlackJackViewModel : ObservableObject, IGame
    {
        private List<Setting> _settings;
        public string PlayerCash{ get => _settings.First(p => p.Key == "Cash").Value + " €"; }
        public string PlayerName { get => _settings.First(p => p.Key == "Name").Value; }     

        public PlayerManager PlayerManager { get; set; }

        public CardManager CardManager { get; set; }


        private ObservableCollection<Card> _playerCards;
        public ObservableCollection<Card> PlayerCards
        {
            get => _playerCards;
            set => SetField(ref _playerCards, value);
        }

        public BlackJackViewModel(List<Setting> settings)
        {
            _settings = settings;
            PlayerManager = new PlayerManager();
            CardManager = new CardManager(Enums.Games.Blackjack);
            PlayerManager.GenerateHumanPlayer(PlayerName);

            PlayerCards = new ObservableCollection<Card>( CardManager.Cards.Take(3));

        }

        public void PlayGame()
        {
            throw new NotImplementedException();
        }
    }
}
