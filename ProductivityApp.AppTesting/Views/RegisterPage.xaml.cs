using System;

using ProductivityApp.AppTesting.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.AppTesting.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterViewModel ViewModel => ViewModelLocator.Current.RegisterViewModel;
        public RegisterPage() => InitializeComponent();
    }
}
