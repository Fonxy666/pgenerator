using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Response;
using PGenerator.Service.InformationService;
using PGenerator.View;

namespace PGenerator.ViewModel;

public class DatabaseViewModel : NotifyPropertyChangedHandler
{
    private readonly IInformationService _informationService;
    private readonly Guid _userId;
    private ICommand _addCommand;
    private readonly byte[] _secretKey;
    private readonly byte[] _iv;

    public DatabaseViewModel() { }
    public DatabaseViewModel(IInformationService informationService, Guid userId, byte[] secretKey, byte[] iv)
    {
        _informationService = informationService;
        _userId = userId;
        _secretKey = secretKey;
        _iv = iv;
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
            NotifyPropertyChanged(nameof(Information));
        }
    }
    
    private Information _selectedInformation;

    public Information SelectedInformation
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
    }
}