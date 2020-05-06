using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.MVVM;

namespace Kartenspiel.Views
{
    public class SettingsObjectViewModel : ObservableObject
    {
        private string _key;
        private string _value;
        public string Key { get => _key; set => SetField(ref _key, value); }
        public string Value { get => _value; set => SetField(ref _value, value); }

        public SettingsObjectViewModel(string key,string value)
        {
            _key = key;
            _value = value;
        }
    }
}
