using System;

using ProductivityApp.AppTesting.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.AppTesting.Views
{
    public sealed partial class SignInPage : Page
    {
        private SignInViewModel ViewModel
        {
            get { return ViewModelLocator.Current.SignInViewModel; }
        }

        public SignInPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
