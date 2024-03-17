using System.Windows.Input;
using PGenerator.ICommandUpdater;
using PGenerator.Model;
using PGenerator.Service.InformationService;

namespace PGenerator.ViewModel;

public class DatabaseViewModel
{
    private readonly IInformationService _informationService;
    private readonly string _userId;
    private ICommand _addCommand;
    public IList<UserInformation> Information { get; set; }

    public DatabaseViewModel() { }
    public DatabaseViewModel(IInformationService informationService, string userId)
    {
        _informationService = informationService;
        _userId = userId;/*
        Information = FetchData();*/
    }

    /*private IList<UserInformation> FetchData()
    {
        return _informationService.ListInformation(_userId);
    }*/
    
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
        Console.WriteLine("haha");
    }
}