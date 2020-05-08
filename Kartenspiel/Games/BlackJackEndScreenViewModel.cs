using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility.MVVM;

namespace Kartenspiel.Games
{
    public class BlackJackEndScreenViewModel : ObservableObject
    {
        private Action<string> _setNewBet;

        private string _header;

        public string Header
        {
            get => _header;
            set => SetField(ref _header, value);
        }

        private string _newBet;

        public string NewBet
        {
            get => _newBet;
            set
            {
                _setNewBet(value);
                SetField(ref _newBet, value);
            }
        }

        private Action<object> NextRound;

        public ICommand NextRoundCmd => new RelayCommand(NextRound);

        private Action<object> End;

        public ICommand EndCmd => new RelayCommand(End);

        private bool? _won;

        public bool? Won => _won;

        public BlackJackEndScreenViewModel(bool? won, Action<object> nextRound, Action<object> end, Action<string> setNewBet, string currentBet)
        {
            if (!won.HasValue)
                Header = "Unentschieden";
            else
                Header = won.Value ? "Gewonnen" : "Verloren";

            NextRound = nextRound;
            End = end;
            _won = won;
            _setNewBet = setNewBet;
            NewBet = currentBet;
        }
    }
}
