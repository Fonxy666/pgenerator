using System.Windows;
using PGenerator.Service.AuthService;
using PGenerator.Service.InformationService;
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
        public IInformationService InformationService { get; set; }
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IInformationService informationService)
        {
            InitializeComponent();
            UserService = userService;
            TokenService = tokenService;
            TokenStorage = tokenStorage;
            InformationService = informationService;
            DataContext = new LoginViewModel(this, UserService, TokenService, TokenStorage, InformationService);
        }
    }
}