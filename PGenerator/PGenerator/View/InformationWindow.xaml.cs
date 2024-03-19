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

    public InformationWindow(Information information, IInformationService informationService, byte[] secretKey,
        byte[] iv)
    {
        InitializeComponent();
        DataContext = new InformationViewModel(information, this, informationService, secretKey, iv);
    }
}