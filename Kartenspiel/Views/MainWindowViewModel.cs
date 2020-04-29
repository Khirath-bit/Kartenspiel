using System.Collections.Generic;
using System.Windows.Controls;
using Kartenspiel.DataObjects;
using Utility.MVVM;

namespace Kartenspiel.Views
{
    public class MainWindowViewModel : ObservableObject
    {
        private UserControl _control = new Settings(new List<Setting>()
        {
            new Setting{Key = "Name"},
            new Setting{Key = "Spieleranzahl"}
        });

        public UserControl Control
        {
            get => _control;
            set => SetField(ref _control, value);
        }

        public void Init()
        {
            Mediator.RegisterEnums(Enums.MediatorEnums.ChangeView, SetControl);
        }

        public void SetControl(object control)
        {
            if (control is UserControl uc)
                Control = uc;
        }
    }
}
