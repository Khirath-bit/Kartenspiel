using Kartenspiel.Manager;
using System.Collections.Generic;

namespace Kartenspiel.DataObjects
{
    public interface IGame
    {
        PlayerManager PlayerManager { get; set; }

        CardManager CardManager { get; set; }

        void PlayGame();
    }
}
