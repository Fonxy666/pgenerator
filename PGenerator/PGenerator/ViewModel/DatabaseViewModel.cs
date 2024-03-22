using System.Windows;
using System.Windows.Input;
using PGenerator.CommandUpdater;
using PGenerator.Model;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.PasswordService;
using PGenerator.View;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PGenerator.ViewModel;

public class DatabaseViewModel : NotifyPropertyChangedHandler
{
    private readonly IAccountDetailService _accountDetailService;
    private readonly Guid _userId;
    private ICommand _addCommand;
    private ICommand _updateCommand;
    private ICommand _deleteCommand;
    private ICommand _filterCommand;
    private ICommand _closeApplication;
    private ICommand _logoutCommand;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;
    private IList<AccountDetailShow> _accountDetail;
    private AccountDetailShow _selectedInformation;
    private string _accountDetailCount;
    private readonly Window _window;
    private readonly Window _loginWindow;
    
    private string _filterText;

    public DatabaseViewModel() { }
    public DatabaseViewModel(IAccountDetailService accountDetailService, Guid userId, byte[] secretKey, byte[] iv, Window window, Window loginWindow)
    {
        _window = window;
        _loginWindow = loginWindow;
        _accountDetailService = accountDetailService;
        _userId = userId;
        _secretKey = secretKey;
        _iv = iv;
        SelectedInformation = new AccountDetailShow(Guid.NewGuid(), string.Empty, string.Empty, string.Empty, DateTime.Now);
        AccountDetail = new List<AccountDetailShow>();
        _accountDetailCount = $"{AccountDetail.Count} added accounts.";
        FetchData();
    }
    
    public IList<AccountDetailShow> AccountDetail
    {
        get => _accountDetail;
        set
        {
            _accountDetail = value;
            NotifyPropertyChanged("AccountDetail");
        }
    }
    
    public AccountDetailShow SelectedInformation
    {
        get => _selectedInformation;
        set
        {
            _selectedInformation = value;
            NotifyPropertyChanged(nameof(SelectedInformation));
        }
    }

    private void FetchData()
    {
        AccountDetail = _accountDetailService.ListDetails(_userId);
        _accountDetailCount = $"{AccountDetail.Count} added accounts.";
        AccountDetailCount = _accountDetailCount;
        NotifyPropertyChanged(nameof(AccountDetail));
    }
    
    public ICommand AddCommand
    {
        get
        {
            if (_addCommand == null)
            {
                _addCommand = new RelayCommand(param => AddNewInfo(), null);
            }

            return _addCommand;
        }
    }

    private void AddNewInfo()
    {
        var informationWindow = new AccountDetailsModalWindow(_userId, _accountDetailService, _secretKey, _iv);
        informationWindow.ShowDialog();
        FetchData();
    }
    
    
    public string FilterText
    {
        get { return _filterText; }
        set
        {
            if (_filterText != value)
            {
                _filterText = value;
                NotifyPropertyChanged(nameof(FilterText));
            }
        }
    }
    
    public ICommand FilterCommand
    {
        get
        {
            if (_filterCommand == null)
            {
                _filterCommand = new RelayCommand(param => FilterMethod(), null);
            }

            return _filterCommand;
        }
    }

    private void FilterMethod()
    {
        FetchData();
        if (FilterText != string.Empty)
        {
            AccountDetail = _accountDetailService.FilterDetails(FilterText);
        }
        else
        {
            FetchData();
        }
    }

    public ICommand UpdateCommand
    {
        get
        {
            if (_updateCommand == null)
            {
                _updateCommand = new RelayCommand(param => UpdateInfo(), null);
            }

            return _updateCommand;
        }
    }

    private void UpdateInfo()
    {
        var newInfo = new AccountDetail(_userId, SelectedInformation.Application, SelectedInformation.Username, PasswordEncrypt.EncryptStringToBytes_Aes(SelectedInformation.Password, _secretKey, _iv))
        {
            Id = SelectedInformation.InfoId
        };
        var informationWindow = new AccountDetailsModalWindow(newInfo, _accountDetailService, _secretKey, _iv);
        informationWindow.ShowDialog();
        FetchData();
    }

    public ICommand DeleteCommand
    {
        get
        {
            if (_deleteCommand == null)
            {
                _deleteCommand = new RelayCommand(param => DeleteInfo(), null);
            }

            return _deleteCommand;
        }
    }

    private async void DeleteInfo()
    {
        var result = await _accountDetailService.DeleteInfo(SelectedInformation.InfoId);
        if (result.Success)
        {
            MessageBox.Show($"Info for application: {SelectedInformation.Application} got deleted successfully.");
            FetchData();
        }
    }
    
    public string AccountDetailCount
    {
        get => _accountDetailCount;
        set
        {
            _accountDetailCount = value; NotifyPropertyChanged("AccountDetailCount");
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
        Application.Current.Shutdown();
    }
    
    public ICommand LogoutCommand
    {
        get
        {
            if (_logoutCommand == null)
            {
                _logoutCommand = new RelayCommand(param => LogoutMethod(), null);
            }

            return _logoutCommand;
        }
    }

    private void LogoutMethod()
    {
        _loginWindow.Visibility = Visibility.Visible;
        _window.Close();
    }
}