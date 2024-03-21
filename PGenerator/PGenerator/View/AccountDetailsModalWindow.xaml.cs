using System.Windows;
using PGenerator.Model;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class AccountDetailsModalWindow : Window
{
    private string actualPassword = "";
    
    public AccountDetailsModalWindow(Guid userId, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
    {
        InitializeComponent();
        DataContext = new AccountDetailViewModel(userId, this, accountDetailService, secretKey, iv);
    }

    public AccountDetailsModalWindow(AccountDetail accountDetail, IAccountDetailService accountDetailService, byte[] secretKey,
        byte[] iv)
    {
        InitializeComponent();
        DataContext = new AccountDetailViewModel(accountDetail, this, accountDetailService, secretKey, iv);
    }
}