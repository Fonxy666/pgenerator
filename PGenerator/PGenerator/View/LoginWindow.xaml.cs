using System.Windows;
using System.Windows.Input;
using PGenerator.Service.AuthService;
using PGenerator.Service.InformationService;
using PGenerator.Service.UserManager;
using PGenerator.TokenStorageFolder;
using PGenerator.ViewModel;

namespace PGenerator.View
{
    public partial class LoginWindow : Window
    {
        private string actualPassword = "";
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this, userService, tokenService, tokenStorage, accountDetailService, secretKey, iv);
        }

        private void Passwordtxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Passwordtxt.Text += "●";
            actualPassword += e.Text;
            Passwordtxt.CaretIndex = Passwordtxt.Text.Length;
            e.Handled = true;

            ((LoginViewModel)DataContext).SetPassword(actualPassword);
        }

        private void Passwordtxt_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (!string.IsNullOrEmpty(actualPassword))
                {
                    actualPassword = actualPassword.Remove(actualPassword.Length - 1);
                }
            }
        }
    }
}