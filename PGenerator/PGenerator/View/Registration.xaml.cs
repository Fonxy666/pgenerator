using System.Windows;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class Registration : Window
{
    public Registration()
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel(this);
    }
}