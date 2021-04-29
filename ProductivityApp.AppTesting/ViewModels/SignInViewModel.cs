using System;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProductivityApp.AppTesting.Helpers;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        private string _email;
        private string _password;

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email
        {
            get => _email;
            set
            {
                if (string.Equals(_email, value)) return;

                _email = value;
                //RaisePropertyChanged("Email");
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password
        {
            get => _password;
            set
            {
                if (string.Equals(_password, value)) return;

                _password = value;
                //RaisePropertyChanged("Password");
            }
        }

        public SignInViewModel()
        {
            RegisterCommand = new Helpers.RelayCommand<string>(test =>
            {
                SimpleMethod();
            });

            LoginCommand = new Helpers.RelayCommand<string>(login =>
            {
                Debug.Print(_password + " " + _email);
                MenuNavigationHelper.UpdateView(typeof(RegisterViewModel).FullName);
            });
        }

        private static void SimpleMethod()
        {
            MenuNavigationHelper.UpdateView(typeof(RegisterViewModel).FullName);
        }
    }
}
