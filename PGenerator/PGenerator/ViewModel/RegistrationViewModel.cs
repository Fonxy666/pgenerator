using System.Windows;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Request;

namespace PGenerator.ViewModel;

public class RegistrationViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    
    public RegistrationViewModel() {}
    
    public RegistrationViewModel(Window window)
    {
        _window = window;
        RegistrationRequest = new RegistrationRequest("", "", "");
    }
    
    private RegistrationRequest? _registrationRequest;

    public RegistrationRequest RegistrationRequest
    {
        get => _registrationRequest;
        set
        {
            _registrationRequest = value; NotifyPropertyChanged(nameof(RegistrationRequest));
        }
    }
    
    private ICommand _registrationCommand;

    public ICommand RegistrationCommand
    {
        get
        {
            if (_registrationCommand == null)
            {
                _registrationCommand = new RelayCommand(param => Registration(), null);
            }

            return _registrationCommand;
        }
    }

    private void Registration()
    {
        if (CheckPasswordsMatch())
        {
            BackMethod();
        }
        else
        {
            ErrorMessageVisibility = Visibility.Visible;
        }
    }
    
    private ICommand _backCommand;

    public ICommand BackCommand
    {
        get
        {
            if (_backCommand == null)
            {
                _backCommand = new RelayCommand(param => BackMethod(), null);
            }

            return _backCommand;
        }
    }

    private void BackMethod()
    {
        _window.Close();
    }

    private bool CheckPasswordsMatch()
    {
        var returningValue = false;
        
        if (Password != null && PasswordRepeat != null)
        {
            returningValue = Password == PasswordRepeat;
        }
        
        return returningValue;
    }
    
    private string _password;
    public string Password
    {
        get => _password;
        set { _password = value; NotifyPropertyChanged("Password"); }
    }
    
    private string _passwordRepeat;
    public string PasswordRepeat
    {
        get => _passwordRepeat;
        set { _passwordRepeat = value; NotifyPropertyChanged("PasswordRepeat"); }
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
}