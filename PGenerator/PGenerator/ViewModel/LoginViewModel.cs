using System.Windows;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Request;
using PGenerator.Service.AuthService;
using PGenerator.Service.InformationService;
using PGenerator.Service.UserManager;
using PGenerator.TokenStorageFolder;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class LoginViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ITokenStorage _tokenStorage;
    private readonly IInformationService _informationService;
    public LoginViewModel() { }

    public LoginViewModel(Window window, IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IInformationService informationService)
    {
        _window = window;
        _userService = userService;
        _tokenService = tokenService;
        _tokenStorage = tokenStorage;
        _informationService = informationService;
        _loginRequest = new LoginRequest(string.Empty, string.Empty);
    }

    private LoginRequest _loginRequest;
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
        var registrationWindow = new RegistrationWindow(_userService);
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
        var result = await _userService.Login(LoginRequest);
        if (result.Success)
        {
            var jwtToken = _tokenService.CreateJwtToken(result.User!, "User");
            await _tokenStorage.SaveTokenAsync(jwtToken);

            if (Guid.TryParse(result.User!.Id, out var guid))
            {
                var databaseWindow = new DatabaseWindow(_informationService, guid);
                databaseWindow.ShowDialog();
                
                _window.Close();
                ErrorMessageVisibility = Visibility.Hidden;
            }
        }
        else
        {
            ErrorMessage = result.Message!;
            ErrorMessageVisibility = Visibility.Visible;
        }
    }
}
