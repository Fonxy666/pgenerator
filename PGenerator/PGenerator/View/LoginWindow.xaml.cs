using System.Windows;
using PGenerator.Service.AuthService;
using PGenerator.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View
{
    public partial class LoginWindow : Window
    {
        public IUserService UserService { get; set; }
        public ITokenService TokenService { get; set; }
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService)
        {
            InitializeComponent();
            UserService = userService;
            TokenService = tokenService;
            DataContext = new LoginViewModel(this, UserService, TokenService);
        }
    }
}