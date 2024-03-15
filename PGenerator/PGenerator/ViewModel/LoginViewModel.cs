using System.Windows;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Service.UserManager;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class LoginViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    private readonly IUserService _userService;
    public LoginViewModel() { }

    public LoginViewModel(Window window, IUserService userService)
    {
        _window = window;
        _userService = userService;
    }
    
    private string _userName;

    public string UserName
    {
        get => _userName; 
        set
        {
            if (_userName != value)
            {
                _userName = value;
            }
        }
    }
    
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
    
    private RelayCommand _showRegisterModal;
    public ICommand ShowRegisterModal
    {
        get
        {
            if (_showRegisterModal == null)
            {
                _showRegisterModal = new RelayCommand(param => ShowRegistrationModal(), null);
            }
            return _showRegisterModal;
        }
    }

    public void ShowRegistrationModal()
    {
        var registrationWindow = new Registration(_userService);
        registrationWindow.ShowDialog();
    }
    
    private RelayCommand _loginCommand;

    public ICommand LoginCommand
    {
        get
        {
            if (_loginCommand == null)
            {
                _loginCommand = new RelayCommand(param => Login(), null);
            }
            return _loginCommand;
        }
    }

    private void Login()
    {
        var haha = _userService.ListUsers().Result[0].UserName;
        Console.WriteLine(haha);
    }
}
