using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Service.InformationService;

namespace PGenerator.ViewModel;

public class DatabaseViewModel : NotifyPropertyChangedHandler
{
    private readonly IInformationService _informationService;
    private readonly Guid _userId;
    private ICommand _addCommand;
    public IList<Information> Information { get; set; }

    public DatabaseViewModel() { }
    public DatabaseViewModel(IInformationService informationService, Guid userId)
    {
        _informationService = informationService;
        _userId = userId;
        FetchData();
    }
    
    private Information _selectedInformation;

    public Information SelectedInformation
    {
        get => _selectedInformation;
        set
        {
            _selectedInformation = value; NotifyPropertyChanged(nameof(SelectedInformation));
        }
    }

    private async Task FetchData()
    {
        Information = await _informationService.ListInformation(_userId);
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
        
    }
}