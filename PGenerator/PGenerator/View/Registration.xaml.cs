using System.Windows;

namespace PGenerator.View;

public partial class Registration : Window
{
    public Registration()
    {
        InitializeComponent();
    }
    
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}