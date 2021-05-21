using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ProductivityApp.App.DataAccess;
using ProductivityApp.App.Helpers;
using ProductivityApp.App.Views;
using ProductivityApp.Model;

namespace ProductivityApp.App.ViewModels
{
    public class RegisterViewModel : ObservableObject
    {
        public ICommand RegisterButtonCommand;

        private string _emailField;
        private string _firstNameField;
        private string _lastNameField;
        private string _passwordField;
        private string _passwordConfirmedField;
        private DateTimeOffset _dateOfBirthField;

        private readonly CrudOperations _dataAccess = new CrudOperations("http://localhost:60098/api", new HttpClient());

        public RegisterViewModel()
        {
            RegisterButtonCommand = new AsyncRelayCommand(RegisterAsync);
        }

        /// <summary>
        /// Registers the User
        /// </summary>
        private async Task RegisterAsync()
        {
            if (!PasswordField.Equals(PasswordConfirmedField))
                return;

            var user = new User()
            {
                EmailAddress = EmailField,
                FirstName = FirstNameField,
                LastName = LastNameField,
                DateOfBirth = DateOfBirthField.DateTime,
                Password = PasswordField
            };

            var created = await _dataAccess.AddEntryToDatabase(user);

            if(created)
                MenuNavigationHelper.UpdateView(typeof(SignInPage));

        }

        public string EmailField
        {
            get => _emailField;
            set => SetProperty(ref _emailField, value);
        }

        public string FirstNameField
        {
            get => _firstNameField;
            set => SetProperty(ref _firstNameField, value);
        }

        public string LastNameField
        {
            get => _lastNameField;
            set => SetProperty(ref _lastNameField, value);
        }
        public string PasswordField
        {
            get => _passwordField;
            set => SetProperty(ref _passwordField, value);
        }
        public string PasswordConfirmedField
        {
            get => _passwordConfirmedField;
            set => SetProperty(ref _passwordConfirmedField, value);
        }
        public DateTimeOffset DateOfBirthField
        {
            get => _dateOfBirthField;
            set => SetProperty(ref _dateOfBirthField, value);
        }
    }
}
