using System.Windows;

namespace PGenerator.View;

public partial class Registration : Window
{
    public Registration()
    {
        InitializeComponent();
    }
    
    private void Registration_Click(object sender, RoutedEventArgs e)
    {
        if (CheckPasswordsMatch())
        {
            Close();
        }
        else
        {
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
    
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModel.UserViewModel viewModel)
        {
            viewModel.Password = ((System.Windows.Controls.PasswordBox)sender).Password;
        }
    }
    
    private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModel.UserViewModel viewModel)
        {
            viewModel.Password = ((System.Windows.Controls.PasswordBox)sender).Password;
        }
    }
    
    private bool CheckPasswordsMatch()
    {
        var returningValue = false;
        var passwordBox = (System.Windows.Controls.PasswordBox)FindName("Passwordtxt")!;
        var repeatPasswordBox = (System.Windows.Controls.PasswordBox)FindName("RepeatPasswordtxt")!;
        
        if (passwordBox != null && repeatPasswordBox != null)
        {
            returningValue = passwordBox.Password == repeatPasswordBox.Password;
        }
        
        return returningValue;
    }
}