using System.Windows;

namespace PGenerator.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel.UserViewModel viewModel)
            {
                viewModel.Password = ((System.Windows.Controls.PasswordBox)sender).Password;
            }
        }
    }
}