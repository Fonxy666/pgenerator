using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.View;

namespace PGenerator.ViewModel
{
    public class UserViewModel : NotifyPropertyChangedHandler
    {
        private string _password;

        public string Password
        {
            get => _password; 
            set
            {
                if (_password != value)
                {
                    _password = value;
                }
            }
        }
        
        private RelayCommand _registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new RelayCommand(param => ShowRegistrationModal(), null);
                }
                return _registerCommand;
            }
        }

        public void ShowRegistrationModal()
        {
            var registrationWindow = new Registration();
            registrationWindow.ShowDialog();
        }
    }
}