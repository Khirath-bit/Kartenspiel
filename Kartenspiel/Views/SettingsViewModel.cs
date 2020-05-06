using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kartenspiel.DataObjects;
using Kartenspiel.Games;
using Utility.MVVM;

namespace Kartenspiel.Views
{
    public class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel(List<Setting> settings)
        {
            foreach (var item in settings)
            {
                _settings.Add(new SettingsObjectViewModel(item.Key, item.Value));
            }
        }

        private ObservableCollection<SettingsObjectViewModel> _settings = new ObservableCollection<SettingsObjectViewModel>();

        public ObservableCollection<SettingsObjectViewModel> Settings
        {
            get => _settings;
            set => SetField(ref _settings, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public ICommand StartGame => new RelayCommand(execStarGame, canStartGame);
        

        private bool canStartGame()
        {
            return !_settings.Any(p => string.IsNullOrWhiteSpace(p.Value));
        }

        private void execStarGame(object param)
        {
            Mediator.NotifyEnumColleagues(Enums.MediatorEnums.ChangeView, new BlackJackViewModel(new List<SettingsObjectViewModel>(_settings)));
        }
    }
}
