using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Kartenspiel.DataObjects;
using Utility.MVVM;

namespace Kartenspiel.Views
{
    public class MainWindowViewModel : ObservableObject
    {
        private ObservableObject _control = new SettingsViewModel(new List<Setting>()
        {
            new Setting{Key = "Name", Value = "Ole"},
            new Setting{Key = "Cash", Value = "10000000"}
        });

        public ObservableObject Control
        {
            get => _control;
            set => SetField(ref _control, value);
        }

        public MainWindowViewModel()
        {

        }

        public void Init()
        {
            Mediator.RegisterEnums(Enums.MediatorEnums.ChangeView, SetControl);
        }

        public void SetControl(object control)
        {
            if (control is ObservableObject uc)
                Control = uc;
        }
    }
}
