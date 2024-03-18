using System.Text;
using System.Windows;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Service.InformationService;
using PGenerator.Service.PasswordService;
using PGenerator.Service.PasswordService.PasswordGenerator;

namespace PGenerator.ViewModel;

public class InformationViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    private Information _information;
    private IInformationService _informationService;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;
    public InformationViewModel() { }
    public InformationViewModel(Guid userId, Window window, IInformationService informationService, byte[] secretKey, byte[] iv)
    {
        _window = window;
        _informationService = informationService;
        _information = new Information(userId, string.Empty, string.Empty, Array.Empty<byte>());
        _secretKey = secretKey;
        _iv = iv;
    }
    
    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value; NotifyPropertyChanged(nameof(Password));
        }
    }
    public Information Information
    {
        get => _information;
        set
        {
            _information = value; NotifyPropertyChanged("Information");
        }
    }

    private RelayCommand _addCommand;

    public ICommand AddCommand
    {
        get
        {
            if (_addCommand == null)
            {
                _addCommand = new RelayCommand(param => AddMethod(), null);
            }

            return _addCommand;
        }
    }

    private async void AddMethod()
    {
        var encryptedPassword = PasswordEncrypt.EncryptStringToBytes_Aes(Password, _secretKey, _iv);
        Information.Password = encryptedPassword;
        await _informationService.AddNewInfo(Information);
    }
    
    private RelayCommand _backCommand;
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

    private RelayCommand _generatePasswordCommand;

    public ICommand GeneratePasswordCommand
    {
        get
        {
            if (_generatePasswordCommand == null)
            {
                _generatePasswordCommand = new RelayCommand(param => GeneratePasswordMethod(), null);
            }

            return _generatePasswordCommand;
        }
    }

    private void GeneratePasswordMethod()
    { 
        Password = PasswordGenerator.GeneratePassword();
        NotifyPropertyChanged("Information");
    }
}