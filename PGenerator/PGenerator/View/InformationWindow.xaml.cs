using System.Windows;
using PGenerator.ViewModel;

namespace PGenerator.View;

public partial class InformationWindow : Window
{
    public InformationWindow()
    {
        InitializeComponent();
        DataContext = new InformationViewModel();
    }
}