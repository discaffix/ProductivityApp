using System;

using ProductivityApp.AppTesting.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.AppTesting.Views
{
    public sealed partial class SignInPage : Page
    {
        public SignInViewModel ViewModel => ViewModelLocator.Current.SignInViewModel;
       

        public SignInPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
