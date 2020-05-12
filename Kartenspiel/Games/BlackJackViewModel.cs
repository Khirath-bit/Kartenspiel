using Kartenspiel.DataObjects;
using Kartenspiel.Manager;
using Kartenspiel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utility.MVVM;

namespace Kartenspiel.Games
{
    public class BlackJackViewModel : ObservableObject, IGame
    {
        #region BackingFields
        private ObservableCollection<Card> _dealerCards;
        private object _finScreen;
        private string _bet;
        private bool _hasFinished;
        private bool _standCommandEnabled;
        private bool _getCardCommandEnabled;
        private bool _doubleCommandEnabled;
        #endregion

        #region ViewProperties
        public bool DoubleCommandEnabled
        {
            get => _doubleCommandEnabled;
            set => SetField(ref _doubleCommandEnabled, value);
        }
        public bool StandCommandEnabled { get => _standCommandEnabled; set => SetField(ref _standCommandEnabled, value); }
        public bool GetCardCommandEnabled { get => _getCardCommandEnabled; set => SetField(ref _getCardCommandEnabled, value); }
        public string PlayerCash => "Guthaben: " + _settings["Cash"] + ",00 €";
        public string PlayerName => "User: " + _settings.First(p => p.Key == "Name").Value;
        public string AugenZahl => "Augenzahl: " + PlayerManager.Player.Augenzahl;
        public string BgImage => Path.GetFullPath("CardImages/BG.jpg");
        public string DealerAugenzahl => "Dealer Augenzahl: " + new ObservableCollection<Card>(DealerCards.Where(w => w.Show)).CountValues();
        public ObservableCollection<Card> PlayerCards => new ObservableCollection<Card>(PlayerManager.Player.Cards);
        public object FinScreen
        {
            get => _finScreen;
            set => SetField(ref _finScreen, value);
        }
        public string Bet
        {
            get => _bet;
            set
            {
                var val = value;
                if (!int.TryParse(value, out var asd))
                    val = _bet;

                _settings["FirstBet"] = val;

                SetField(ref _bet, val);
            }
        }
        public ObservableCollection<Card> DealerCards
        {
            get => _dealerCards;
            set => SetField(ref _dealerCards, value);
        }
        #endregion

        #region Commands
        public ICommand GetCardCmd => new RelayCommand(ExecGetCard);
        public ICommand StandCommand => new RelayCommand(ExecStand);

        public ICommand DoubleCommand => new RelayCommand(Double);
        #endregion

        #region CommandMethods
        /// <summary>
        /// Doubles the bet 
        /// </summary>
        private void Double(object param)
        {
            StandCommandEnabled = false;
            GetCardCommandEnabled = false;
            DoubleCommandEnabled = false;

            if (int.TryParse(Bet, out var bet))
                Bet = (bet * 2).ToString();

            ExecGetCard(null);

            if(!_hasFinished)
                ExecStand(null);
        }

        /// <summary>
        /// Executes the stand 
        /// </summary>
        private async void ExecStand(object param)
        {
            DoubleCommandEnabled = false;
            GetCardCommandEnabled = false;
            StandCommandEnabled = false;

            var currCard = DealerCards[1];
            DealerCards[1] = new Card
            {
                Back = currCard.Back,
                CardSigns = currCard.CardSigns,
                Description = currCard.Description,
                Img = currCard.Img,
                Show = true,
                Value = currCard.Value
            };

            await Task.Run(async () =>
            {
                while (!HasDealerFinished())
                {
                    Thread.Sleep(500);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        DealerCards.Add(CardManager.GetCards(1, true)[0]);
                    });

                    while (DealerCards.ToList().Exists(e => e.Value == 11) && DealerCards.CountValues() > 21)
                        DealerCards.First(e => e.Value == 11).Value = 1; //Set Ace's to value 1

                    UpdateView();
                }
                Thread.Sleep(500);

                Finish();
            });
        }

        /// <summary>
        /// Adds a card to the player
        /// </summary>
        /// <param name="param"></param>
        private void ExecGetCard(object param)
        {
            DoubleCommandEnabled = false;

            PlayerManager.Player.Cards.AddRange(CardManager.GetCards(1, true));

            while (PlayerManager.Player.Cards.Exists(e => e.Value == 11) && PlayerManager.Player.Augenzahl > 21)
                PlayerManager.Player.Cards.First(e => e.Value == 11).Value = 1; //Set Ace's to value 1

            if (PlayerManager.Player.Augenzahl > 21)
                Finish();

            if(PlayerManager.Player.Augenzahl == 21)
                ExecStand(null);

            UpdateView();
        }

        /// <summary>
        /// Finishes game and starts the next round
        /// </summary>
        /// <param name="param"></param>
        private void NextRound(object param)
        {
            var won = (bool?)param;

            long.TryParse(_settings["Cash"], out var cash);
            long.TryParse(Bet, out var bet);
            if (won.HasValue && won.Value)
                cash += bet;
            else if (won.HasValue)
                cash -= bet;

            _settings["Cash"] = cash.ToString();

            Mediator.NotifyEnumColleagues(Enums.MediatorEnums.ChangeView, new BlackJackViewModel(_settings.Select(s => new SettingsObjectViewModel(s.Key, s.Value)).ToList()));
        }

        /// <summary>
        /// Ends the program
        /// </summary>
        /// <param name="param"></param>
        private void EndGame(object param)
        {
            Application.Current.Shutdown();
        }
        #endregion

        private Dictionary<string, string> _settings;
        public PlayerManager PlayerManager { get; set; }
        public CardManager CardManager { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BlackJackViewModel(List<SettingsObjectViewModel> settings) => PlayGame(settings);

        /// <summary>
        /// Constructor delegate - Manages the game
        /// </summary>
        public void PlayGame(List<SettingsObjectViewModel> settings)
        {
            GetCardCommandEnabled = true;
            DoubleCommandEnabled = true;
            StandCommandEnabled = true;

            _settings = settings.ToDictionary(s => s.Key, s => s.Value);

            PlayerManager = new PlayerManager();
            CardManager = new CardManager(Enums.Games.Blackjack);

            DealerCards = new ObservableCollection<Card>(CardManager.GetCards(1, true)); // Show first dealer card
            DealerCards.Add(CardManager.GetCards(1)[0]); //Hide second

            PlayerManager.GenerateHumanPlayer(_settings["Name"]);

            PlayerManager.Player.Cards = CardManager.GetCards(2, true);

            Bet = _settings["FirstBet"];

            if (DealerCards[0].Value + DealerCards[1].Value != 21 && PlayerManager.Player.Augenzahl != 21) //If the player or the dealer has a blackjack, finish the game
                return;

            Finish();
        }

        /// <summary>
        /// Sets the new bet
        /// </summary>
        private void SetNewBet(string bet)
        {
            _settings["FirstBet"] = bet;
        }

        /// <summary>
        /// Calculates end results and opens the end screen
        /// </summary>
        private void Finish()
        {
            _hasFinished = true;
            DealerCards[1].Show = true;
            var dealerPoints = DealerCards.CountValues();
            var playerPoints = PlayerManager.Player.Augenzahl;

            bool? won = (playerPoints > dealerPoints && playerPoints <= 21)
                || dealerPoints > 21;

            if (dealerPoints == playerPoints)
                won = null;

            FinScreen = new BlackJackEndScreenViewModel(won, NextRound, EndGame, SetNewBet, Bet);
            UpdateView();
        }

        /// <summary>
        /// Checks if the dealer has finished
        /// </summary>
        private bool HasDealerFinished()
        {
            var sum = DealerCards.CountValues();

            return sum >= 17 && sum > Math.Min(PlayerManager.Player.Augenzahl, 20);
        }

        /// <summary>
        /// Updates the view properties
        /// </summary>
        private void UpdateView()
        {
            OnPropertyChanged("PlayerCards");
            OnPropertyChanged("AugenZahl");
            OnPropertyChanged("DealerCards");
            OnPropertyChanged("PlayerCash");
            OnPropertyChanged("DealerAugenzahl");
            OnPropertyChanged("FinScreen");
        }
    }
}
