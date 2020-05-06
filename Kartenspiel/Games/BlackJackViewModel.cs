using Kartenspiel.DataObjects;
using Kartenspiel.Manager;
using Kartenspiel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        public BlackJackViewModel(List<SettingsObjectViewModel> settings)
        {
            _settings = settings;
            PlayerManager = new PlayerManager();
            CardManager = new CardManager(Enums.Games.Blackjack);
            PlayerManager.GenerateHumanPlayer(PlayerName);

            DealerCards = new ObservableCollection<Card>(CardManager.GetCards(2));
            DealerCards[0].Show = true;

            PlayerManager.Player.Cards = CardManager.GetCards(2, true);
        }

        public void PlayGame()
        {
            throw new NotImplementedException();
        }




        public ICommand GetCardCmd => new RelayCommand(ExecGetCard);

        private void ExecGetCard(object param)
        {
            PlayerManager.Player.Cards.AddRange(CardManager.GetCards(1, true));
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
