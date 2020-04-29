using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Kartenspiel.DataObjects;
using Utility.MVVM;

namespace Kartenspiel.Views
{
    public class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel(List<Setting> settings)
        {
            Settings = new ObservableCollection<Setting>(settings);
        }

        private ObservableCollection<Setting> _settings;

        public ObservableCollection<Setting> Settings
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
    }
}
