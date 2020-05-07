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
        private string _header;

        public string Header
        {
            get => _header;
            set => SetField(ref _header, value);
        }

        private Action<object> NextRound;

        public ICommand NextRoundCmd => new RelayCommand(NextRound);

        private Action<object> End;

        public ICommand EndCmd => new RelayCommand(End);

        private bool? _won;

        public bool? Won => _won;

        public BlackJackEndScreenViewModel(bool? won, Action<object> nextRound, Action<object> end)
        {
            if (!won.HasValue)
                Header = "Unentschieden";
            else
                Header = won.Value ? "Gewonnen" : "Verloren";

            NextRound = nextRound;
            End = end;
            _won = won;
        }
    }
}
