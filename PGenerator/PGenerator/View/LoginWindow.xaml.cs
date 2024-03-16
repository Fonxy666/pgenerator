using System.Windows;
using PGenerator.Service.AuthService;
using PGenerator.Service.UserManager;
using PGenerator.TokenStorageFolder;
using PGenerator.ViewModel;

namespace PGenerator.View
{
    public partial class LoginWindow : Window
    {
        public IUserService UserService { get; set; }
        public ITokenService TokenService { get; set; }
        public ITokenStorage TokenStorage { get; set; }
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage)
        {
            InitializeComponent();
            UserService = userService;
            TokenService = tokenService;
            TokenStorage = tokenStorage;
            DataContext = new LoginViewModel(this, UserService, TokenService, TokenStorage);
        }
    }
}