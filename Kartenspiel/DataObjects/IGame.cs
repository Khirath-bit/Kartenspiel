using Kartenspiel.Manager;
using System.Collections.Generic;
using Kartenspiel.Views;

namespace Kartenspiel.DataObjects
{
    public interface IGame
    {
        PlayerManager PlayerManager { get; set; }

        CardManager CardManager { get; set; }

        void PlayGame(List<SettingsObjectViewModel> settings = default);
    }
}
