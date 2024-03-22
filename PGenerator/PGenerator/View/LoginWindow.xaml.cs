using System.Windows;
using PGenerator.Data.TokenStorageFolder;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.AuthService;
using PGenerator.Model.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this, userService, tokenService, tokenStorage, accountDetailService, secretKey, iv);
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).PasswordVisibility = !((LoginViewModel)DataContext).PasswordVisibility;
            Console.WriteLine(((LoginViewModel)DataContext).PasswordVisibility);
        }
    }
}