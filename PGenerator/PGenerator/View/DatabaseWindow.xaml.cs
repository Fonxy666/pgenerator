using System.Windows;
using System.Windows.Input;
using PGenerator.Service.InformationService;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class DatabaseWindow : Window
{
    public DatabaseWindow() { }
    public DatabaseWindow(IAccountDetailService accountDetailService, Guid userId, byte[] secretKey, byte[] iv)
    {
        InitializeComponent();
        DataContext = new DatabaseViewModel(accountDetailService, userId, secretKey, iv);
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private bool IsMaximized = false;
    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            if (IsMaximized)
            {
                WindowState = WindowState.Normal;
                Width = 1080;
                Height = 720;
                IsMaximized = false;
            }
            else
            {
                WindowState = WindowState.Maximized;
                IsMaximized = true;
            }
        }
    }
}