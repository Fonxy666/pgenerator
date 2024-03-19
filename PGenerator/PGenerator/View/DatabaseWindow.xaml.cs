using System.Windows;
using PGenerator.Service.InformationService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow() { }
    public DatabaseWindow(IAccountDetailService accountDetailService, Guid userId, byte[] secretKey, byte[] iv)
    {
        InitializeComponent();
        DataContext = new DatabaseViewModel(accountDetailService, userId, secretKey, iv);
    }
}