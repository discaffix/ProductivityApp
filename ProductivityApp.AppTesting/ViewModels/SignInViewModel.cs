using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProductivityApp.AppTesting.Helpers;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        public ICommand SimpleCommand { get; set; }
 
        public SignInViewModel()
        {
            // TODO: Add Command for login
            SimpleCommand = new Helpers.RelayCommand<string>(test =>
            {
                SimpleMethod();
            });
        }

        private static void SimpleMethod()
        {
            MenuNavigationHelper.UpdateView(typeof(RegisterViewModel).FullName);
        }
    }
}
