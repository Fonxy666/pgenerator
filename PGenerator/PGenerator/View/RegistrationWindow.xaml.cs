using System.Windows;
using PGenerator.Model.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class RegistrationWindow : Window
{
    public IUserService UserService { get; set; }

    public RegistrationWindow() { }
    public RegistrationWindow(IUserService userService)
    {
        InitializeComponent();
        UserService = userService;
        DataContext = new RegistrationViewModel(this, UserService);
    }
}