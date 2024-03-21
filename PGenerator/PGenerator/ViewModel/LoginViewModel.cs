using System.Windows;
using System.Windows.Input;
using PGenerator.CommandUpdater;
using PGenerator.Data.TokenStorageFolder;
using PGenerator.Model.Request;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.AuthService;
using PGenerator.Model.Service.UserManager;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class LoginViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ITokenStorage _tokenStorage;
    private readonly IAccountDetailService _accountDetailService;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;
    private LoginRequest _loginRequest;
    private ICommand _showRegisterModal;
    private ICommand _loginCommand;
    private string _errorMessage;
    private Visibility _errorMessageVisibility = Visibility.Collapsed;
    private ICommand _closeApplication;
    private string _actualPassword;
    
    public LoginViewModel() { }

    public LoginViewModel(Window window, IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
    {
        _window = window;
        _userService = userService;
        _tokenService = tokenService;
        _tokenStorage = tokenStorage;
        _accountDetailService = accountDetailService;
        _secretKey = secretKey;
        _iv = iv;
        _loginRequest = new LoginRequest(string.Empty, string.Empty);
    }
    
    public LoginRequest LoginRequest
    {
        get => _loginRequest; 
        set
        {
            if (_loginRequest != value)
            {
                _loginRequest = value;
            }
        }
    }
    
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
        var registrationWindow = new RegistrationWindow(_userService);
        registrationWindow.ShowDialog();
    }
    
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
    
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value; NotifyPropertyChanged(nameof(ErrorMessage));
        }
    }
    
    public Visibility ErrorMessageVisibility
    {
        get => _errorMessageVisibility;
        set
        {
            _errorMessageVisibility = value;
            NotifyPropertyChanged(nameof(ErrorMessageVisibility));
        }
    }
    
    public void SetPassword(string password)
    {
        _actualPassword = password;
    }

    private async void Login()
    {
        var request = new LoginRequest(LoginRequest.UserName, _actualPassword);
        var result = await _userService.Login(request);
        if (result.Success)
        {
            var jwtToken = _tokenService.CreateJwtToken(result.User!, "User");
            await _tokenStorage.SaveTokenAsync(jwtToken);

            if (Guid.TryParse(result.User!.Id, out var guid))
            {
                var databaseWindow = new DatabaseWindow(_accountDetailService, guid, _secretKey, _iv);
                ErrorMessageVisibility = Visibility.Hidden;
                _window.Close();
                
                databaseWindow.ShowDialog();
            }
        }
        else
        {
            ErrorMessage = result.Message!;
            ErrorMessageVisibility = Visibility.Visible;
        }
    }
    
    public ICommand CloseApplication
    {
        get
        {
            if (_closeApplication == null)
            {
                _closeApplication = new RelayCommand(param => CloseApp(), null);
            }

            return _closeApplication;
        }
    }

    private void CloseApp()
    {
        _window.Close();
    }
}
