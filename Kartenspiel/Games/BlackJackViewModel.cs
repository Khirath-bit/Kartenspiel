using Kartenspiel.DataObjects;
using Kartenspiel.Manager;
using Kartenspiel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility.MVVM;

namespace Kartenspiel.Games
{
    public class BlackJackViewModel : ObservableObject, IGame
    {
        private List<SettingsObjectViewModel> _settings;
        public string PlayerCash{ get => "Guthaben: " + _settings.First(p => p.Key == "Cash").Value + ",00 €"; }
        public string PlayerName { get => "User: " + _settings.First(p => p.Key == "Name").Value; }     
        public string AugenZahl { get => "Augenzahl: " + PlayerManager.Player.Augenzahl; }

        public PlayerManager PlayerManager { get; set; }
        public CardManager CardManager { get; set; }

        private bool _modalAc;
        public bool ModalActivated
        {
            get => _modalAc;
            set
            {
                SetField(ref _modalAc, value);
            }
        }

        public string BgImage { get => Path.GetFullPath("CardImages/BG.jpg"); }
        public ObservableCollection<Card> PlayerCards
        {
            get => new ObservableCollection<Card>(PlayerManager.Player.Cards);
        }
        private ObservableCollection<Card> _dealerCards;
        public ObservableCollection<Card> DealerCards
        {
            get => _dealerCards;
            set => SetField(ref _dealerCards, value);
        }

        private string _bet;

        public string Bet
        {
            get => _bet;
            set
            {
                var val = value;
                if (!int.TryParse(value, out var asd))
                    val = _bet;
                
                SetField(ref _bet, val);
            }
        }

        public BlackJackViewModel(List<SettingsObjectViewModel> settings)
        {
            _settings = settings;
            PlayerManager = new PlayerManager();
            CardManager = new CardManager(Enums.Games.Blackjack);
            PlayerManager.GenerateHumanPlayer(PlayerName);

            DealerCards = new ObservableCollection<Card>(CardManager.GetCards(2));
            DealerCards[0].Show = true;

            PlayerManager.Player.Cards = CardManager.GetCards(2, true);

            Bet = "50";

            ModalActivated = false;

            if (DealerCards[0].Value + DealerCards[1].Value == 21)
            {
                ModalActivated = true;
                DealerCards[1].Show = true;
            }
        }

        public void PlayGame()
        {
            throw new NotImplementedException();
        }

        public bool HasWon()
        {
            return PlayerManager.Player.Augenzahl < 22;
        }

        public ICommand StandCommand => new RelayCommand(ExecStand);

        private void ExecStand(object param)
        {
            DealerCards[1].Show = true; //TODO: Wieso wird das hier nicht gezupdated???

            UpdateView();

            while (!HasDealerFinished())
            {
                DealerCards.Add(CardManager.GetCards(1, true)[0]);
            }
        }

        private bool HasDealerFinished()
        {
            var fin = false;

            var sum = DealerCards.CountValues();

            return sum >= 17;
        }

        public ICommand GetCardCmd => new RelayCommand(ExecGetCard);

        private void ExecGetCard(object param)
        {
            PlayerManager.Player.Cards.AddRange(CardManager.GetCards(1, true));
            if (PlayerManager.Player.Augenzahl > 21)
                ModalActivated = true;
            UpdateView();
        }

        private void UpdateView()
        {
            OnPropertyChanged("PlayerCards");
            OnPropertyChanged("AugenZahl");
            OnPropertyChanged("DealerCards");
            OnPropertyChanged("PlayerCash");
        }
    }
}
