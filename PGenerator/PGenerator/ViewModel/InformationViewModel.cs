using System.Text;
using System.Windows;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Request;
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
    private readonly bool _updateInfo;
    public InformationViewModel() { }
    public InformationViewModel(Guid userId, Window window, IInformationService informationService, byte[] secretKey, byte[] iv)
    {
        _window = window;
        _informationService = informationService;
        _information = new Information(userId, string.Empty, string.Empty, Array.Empty<byte>());
        _secretKey = secretKey;
        _iv = iv;
        _updateInfo = false;
    }
    
    public InformationViewModel(Information information, Window window, IInformationService informationService, byte[] secretKey, byte[] iv)
    {
        _window = window;
        _informationService = informationService;
        _information = information;
        _secretKey = secretKey;
        _iv = iv;
        _updateInfo = true;
        _password = PasswordEncrypt.DecryptStringFromBytes_Aes(information.Password, secretKey, iv);
    }

    public string AccountButton => _updateInfo ? "Update" : "Add";

    public ICommand AccountButtonCommand => _updateInfo ? UpdateCommand : AddCommand;

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value; NotifyPropertyChanged("Password");
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
        var result = await _informationService.AddNewInfo(Information);
        if (result.Success)
        {
            MessageBox.Show("New account successfully added.");
            _window.Close();
        }
    }
    
    private RelayCommand _updateCommand;

    public ICommand UpdateCommand
    {
        get
        {
            if (_updateCommand == null)
            {
                _updateCommand = new RelayCommand(param => UpdateMethod(), null);
            }

            return _updateCommand;
        }
    }

    private async void UpdateMethod()
    {
        Console.WriteLine(Password);
        var request = new UpdateRequest(_information.Application!, _information.UserName!, Password);
        var result = await _informationService.UpdateInfo(request, _information.Id);
        if (result.Success)
        {
            MessageBox.Show($"Account for: {_information.Application} application successfully updated.");
            _window.Close();
        }
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