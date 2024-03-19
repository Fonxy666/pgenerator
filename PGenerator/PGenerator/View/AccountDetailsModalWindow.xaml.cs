using System.Windows;
using PGenerator.Model;
using PGenerator.Service.InformationService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class AccountDetailsModalWindow : Window
{
    public AccountDetailsModalWindow(Guid userId, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
    {
        InitializeComponent();
        DataContext = new InformationViewModel(userId, this, accountDetailService, secretKey, iv);
    }

    public AccountDetailsModalWindow(AccountDetail accountDetail, IAccountDetailService accountDetailService, byte[] secretKey,
        byte[] iv)
    {
        InitializeComponent();
        DataContext = new InformationViewModel(accountDetail, this, accountDetailService, secretKey, iv);
    }
}