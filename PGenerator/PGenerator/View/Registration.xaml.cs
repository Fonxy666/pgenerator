using System.Windows;
using PGenerator.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class Registration : Window
{
    public IUserService UserService { get; set; }

    public Registration() { }
    public Registration(IUserService userService)
    {
        InitializeComponent();
        UserService = userService;
        DataContext = new RegistrationViewModel(this, UserService);
    }
}