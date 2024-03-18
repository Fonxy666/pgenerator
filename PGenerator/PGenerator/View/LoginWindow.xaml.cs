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
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IInformationService informationService, byte[] secretKey, byte[] iv)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this, userService, tokenService, tokenStorage, informationService, secretKey, iv);
        }
    }
}