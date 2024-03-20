using System.Windows.Forms;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Request;
using PGenerator.Response;
using PGenerator.Service.InformationService;
using PGenerator.Service.PasswordService;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class DatabaseViewModel : NotifyPropertyChangedHandler
{
    private readonly IAccountDetailService _accountDetailService;
    private readonly Guid _userId;
    private ICommand _addCommand;
    private ICommand _updateCommand;
    private ICommand _deleteCommand;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;
    private IList<AccountDetailShow> _accountDetail;
    private AccountDetailShow _selectedInformation;
    private string _accountDetailCount;

    public DatabaseViewModel() { }
    public DatabaseViewModel(IAccountDetailService accountDetailService, Guid userId, byte[] secretKey, byte[] iv)
    {
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
        AccountDetail = _accountDetailService.ListInformation(_userId);
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
}