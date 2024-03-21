using System.Windows;
using System.Windows.Input;
using PGenerator.CommandUpdater;
using PGenerator.Model;
using PGenerator.Model.Request;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.PasswordService;
using PGenerator.Model.Service.PasswordService.PasswordGenerator;

namespace PGenerator.ViewModel;

public class AccountDetailViewModel : NotifyPropertyChangedHandler
{
    private readonly Window _window;
    private AccountDetail _accountDetail;
    private IAccountDetailService _accountDetailService;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;
    private readonly bool _updateInfo;
    private string _password;
    private ICommand _addCommand;
    private ICommand _updateCommand;
    private ICommand _backCommand;
    private ICommand _generatePasswordCommand;
    
    public AccountDetailViewModel() { }
    public AccountDetailViewModel(Guid userId, Window window, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
    {
        _window = window;
        _accountDetailService = accountDetailService;
        _accountDetail = new AccountDetail(userId, string.Empty, string.Empty, Array.Empty<byte>());
        _secretKey = secretKey;
        _iv = iv;
        _updateInfo = false;
    }
    
    public AccountDetailViewModel(AccountDetail accountDetail, Window window, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
    {
        _window = window;
        _accountDetailService = accountDetailService;
        _accountDetail = accountDetail;
        _secretKey = secretKey;
        _iv = iv;
        _updateInfo = true;
        _password = PasswordEncrypt.DecryptStringFromBytes_Aes(accountDetail.Password, secretKey, iv);
    }

    public string AccountButton => _updateInfo ? "Update" : "Add";

    public ICommand AccountButtonCommand => _updateInfo ? UpdateCommand : AddCommand;
    
    public string Password
    {
        get => _password;
        set
        {
            _password = value; NotifyPropertyChanged("Password");
        }
    }
    public AccountDetail AccountDetail
    {
        get => _accountDetail;
        set
        {
            _accountDetail = value; NotifyPropertyChanged("Information");
        }
    }
    
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
        AccountDetail.Password = encryptedPassword;
        var result = await _accountDetailService.AddNewInfo(AccountDetail);
        if (result.Success)
        {
            MessageBox.Show("New account successfully added.");
            _window.Close();
        }
    }
    
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
        var request = new UpdateRequest(_accountDetail.Application!, _accountDetail.UserName!, Password);
        var result = await _accountDetailService.UpdateInfo(request, _accountDetail.Id);
        if (result.Success)
        {
            MessageBox.Show($"Account for: {_accountDetail.Application} application successfully updated.");
            _window.Close();
        }
    }
    
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