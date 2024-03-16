using System.Windows;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Request;
using PGenerator.Service.AuthService;
using PGenerator.Service.UserManager;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class LoginViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    public LoginViewModel() { }

    public LoginViewModel(Window window, IUserService userService, ITokenService tokenService)
    {
        _window = window;
        _userService = userService;
        _tokenService = tokenService;
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
    
    private string _errorMessage;

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value; NotifyPropertyChanged(nameof(ErrorMessage));
        }
    }
    
    private Visibility _errorMessageVisibility = Visibility.Collapsed;

    public Visibility ErrorMessageVisibility
    {
        get => _errorMessageVisibility;
        set
        {
            _errorMessageVisibility = value;
            NotifyPropertyChanged(nameof(ErrorMessageVisibility));
        }
    }

    private async void Login()
    {
        var request = new LoginRequest(UserName, Password);
        var result = await _userService.Login(request);
        if (result.Success)
        {
            ErrorMessageVisibility = Visibility.Hidden;
        }
        else
        {
            ErrorMessage = result.Message;
            ErrorMessageVisibility = Visibility.Visible;
        }
    }
}
