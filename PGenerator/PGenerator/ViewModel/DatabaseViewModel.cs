using System.Windows.Forms;
using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Request;
using PGenerator.Response;
using PGenerator.Service.InformationService;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class DatabaseViewModel : NotifyPropertyChangedHandler
{
    private readonly IInformationService _informationService;
    private readonly Guid _userId;
    private ICommand _addCommand;
    private ICommand _updateCommand;
    private ICommand _deleteCommand;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;

    public DatabaseViewModel() { }
    public DatabaseViewModel(IInformationService informationService, Guid userId, byte[] secretKey, byte[] iv)
    {
        _informationService = informationService;
        _userId = userId;
        _secretKey = secretKey;
        _iv = iv;
        SelectedInformation = new Database(Guid.NewGuid(), string.Empty, string.Empty, string.Empty, DateTime.Now);
        Information = new List<Database>();
        FetchData();
    }
    
    private IList<Database> _information;

    public IList<Database> Information
    {
        get => _information;
        set
        {
            _information = value;
            NotifyPropertyChanged("Information");
        }
    }
    
    private Database _selectedInformation;

    public Database SelectedInformation
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
        Information = _informationService.ListInformation(_userId);
        NotifyPropertyChanged(nameof(Information));
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
        var informationWindow = new InformationWindow(_userId, _informationService, _secretKey, _iv);
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

    private async void UpdateInfo()
    {
        var updateRequest = new UpdateRequest(SelectedInformation.Application, SelectedInformation.Username,
            SelectedInformation.Password);
        var result =  await _informationService.UpdateInfo(updateRequest, SelectedInformation.InfoId);

        if (result.Success)
        {
            MessageBox.Show($"Info for application: {SelectedInformation.Application} got updated successfully.");
            FetchData();
        }
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
        var result = await _informationService.DeleteInfo(SelectedInformation.InfoId);
        if (result.Success)
        {
            MessageBox.Show($"Info for application: {SelectedInformation.Application} got deleted successfully.");
            FetchData();
        }
    }
}