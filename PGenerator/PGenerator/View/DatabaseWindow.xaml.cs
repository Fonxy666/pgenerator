using System.Windows;
using System.Windows.Input;
using PGenerator.Data.TokenStorageFolder;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.AuthService;
using PGenerator.Model.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow() { }
    public DatabaseWindow(IAccountDetailService accountDetailService, Guid userId, byte[] secretKey, byte[] iv, IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage)
    {
        InitializeComponent();
        DataContext = new DatabaseViewModel(accountDetailService, userId, secretKey, iv, this, userService, tokenService, tokenStorage);
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private bool _isMaximized;
    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            if (_isMaximized)
            {
                WindowState = WindowState.Normal;
                Width = 1080;
                Height = 720;
                _isMaximized = false;
            }
            else
            {
                WindowState = WindowState.Maximized;
                _isMaximized = true;
            }
        }
    }
}