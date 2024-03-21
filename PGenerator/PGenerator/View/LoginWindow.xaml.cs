﻿using System.Windows;
using System.Windows.Input;
using PGenerator.Data.TokenStorageFolder;
using PGenerator.Model.Service.AccountDetailService;
using PGenerator.Model.Service.AuthService;
using PGenerator.Model.Service.UserManager;
using PGenerator.ViewModel;

namespace PGenerator.View
{
    public partial class LoginWindow : Window
    {
        private string actualPassword = "";
        private bool showActualPassword = false;
        public LoginWindow() { }
        
        public LoginWindow(IUserService userService, ITokenService tokenService, ITokenStorage tokenStorage, IAccountDetailService accountDetailService, byte[] secretKey, byte[] iv)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this, userService, tokenService, tokenStorage, accountDetailService, secretKey, iv);
        }

        private void Passwordtxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (((LoginViewModel)DataContext).PasswordVisibility)
            {
                Passwordtxt.Text += e.Text;
                actualPassword += e.Text;
                Passwordtxt.CaretIndex = Passwordtxt.Text.Length;
                e.Handled = true;
                ((LoginViewModel)DataContext).SetPassword(actualPassword);
            }
            else
            {
                Passwordtxt.Text += "●";
                actualPassword += e.Text;
                Passwordtxt.CaretIndex = Passwordtxt.Text.Length;
                e.Handled = true;
                ((LoginViewModel)DataContext).SetPassword(actualPassword);
            }
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

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!((LoginViewModel)DataContext).PasswordVisibility)
            {
                Passwordtxt.Text = actualPassword;
            }
            else
            {
                Passwordtxt.Text = "";
                for (int i = 0; i < actualPassword.Length; i++)
                {
                    Passwordtxt.Text += "●";
                }
            }
        }
    }
}