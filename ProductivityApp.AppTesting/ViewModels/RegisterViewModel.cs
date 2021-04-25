using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ICommand SimpleCommand { get; set; }
        
        public RegisterViewModel()
        {

        }
    }
}
