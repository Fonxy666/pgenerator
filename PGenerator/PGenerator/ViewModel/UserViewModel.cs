namespace PGenerator.ViewModel
{
    public class UserViewModel
    {
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    // NotifyPropertyChanged if you have implemented INotifyPropertyChanged
                }
            }
        }
    }
}