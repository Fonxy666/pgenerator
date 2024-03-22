using System.Windows;
using System.Windows.Input;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow() { }
    public DatabaseWindow(IAccountDetailService accountDetailService, Guid userId, byte[] secretKey, byte[] iv, Window loginWindow)
    {
        InitializeComponent();
        DataContext = new DatabaseViewModel(accountDetailService, userId, secretKey, iv, this,loginWindow);
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