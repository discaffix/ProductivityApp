using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ProductivityApp.App.ViewModels
{
    public class SignInViewModel : ObservableObject
    {
        public ICommand LoginCommand;

        private string _usernameField = string.Empty;
        private string _passwordField = string.Empty;

        public SignInViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private async void Login()
        {
            Debug.WriteLine(UsernameField);
            Debug.WriteLine(PasswordField);
        }


        public string UsernameField
        {
            get => _usernameField;
            set => SetProperty(ref _usernameField, value);
        }

        public string PasswordField
        {
            get => _passwordField;
            set => SetProperty(ref _passwordField, value);
        }
    }
}
