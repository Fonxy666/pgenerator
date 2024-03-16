using System.Windows;
using PGenerator.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View
{
    public partial class LoginWindow : Window
    {
        public IUserService UserService { get; set; }
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService)
        {
            InitializeComponent();
            UserService = userService;
            DataContext = new LoginViewModel(this, UserService);
        }
    }
}