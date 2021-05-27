using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using ProductivityApp.App.DataAccess;
using ProductivityApp.App.Helpers;
using ProductivityApp.App.Views;
using ProductivityApp.Model;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace ProductivityApp.App.ViewModels
{
    public class SignInViewModel : ObservableObject
    {
        public ICommand LoginCommand;
        public ICommand PageLoadedCommand;
        private string _emailField = string.Empty;
        private string _passwordField = string.Empty;

        private readonly CrudOperations _dataAccess = new CrudOperations("http://localhost:60098/api", new HttpClient());


        public SignInViewModel()
        {
            LoginCommand = new AsyncRelayCommand(Login);
            PageLoadedCommand = new RelayCommand(PageLoaded);
        }

        public void PageLoaded()
        {
            ApplicationData.Current.LocalSettings.Values["user"] =
                new ApplicationDataCompositeValue { ["id"] = 0 };
        }

        private async Task Login()
        {
            var users = await _dataAccess.GetDataFromUri<User>("Users");

            var validUser = users
                .Where(u => u.EmailAddress == EmailField)
                .Select(u => u)
                .Where(st => st.Password == PasswordField)
                .Select(u => u)
                .ToList();

            if (validUser.Count != 1) return;

            ApplicationData.Current.LocalSettings.Values["user"] =
                new ApplicationDataCompositeValue() { ["id"] = validUser[0].UserId };

            MenuNavigationHelper.UpdateView(typeof(MainPage));
        }


        public string EmailField
        {
            get => _emailField;
            set => SetProperty(ref _emailField, value);
        }

        public string PasswordField
        {
            get => _passwordField;
            set => SetProperty(ref _passwordField, value);
        }
    }
}
