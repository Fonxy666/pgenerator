using System.Windows;
using PGenerator.Model;
using PGenerator.Service.InformationService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class InformationWindow : Window
{
    public InformationWindow(Guid userId, IInformationService informationService, byte[] secretKey, byte[] iv)
    {
        InitializeComponent();
        DataContext = new InformationViewModel(userId, this, informationService, secretKey, iv);
    }

    public InformationWindow(AccountInformation accountInformation, IInformationService informationService, byte[] secretKey,
        byte[] iv)
    {
        InitializeComponent();
        DataContext = new InformationViewModel(accountInformation, this, informationService, secretKey, iv);
    }
}