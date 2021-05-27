
using ProductivityApp.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.App.Views
{
    public sealed partial class SignInPage : Page
    {
        public SignInViewModel ViewModel { get; } = new SignInViewModel();

        public SignInPage()
        {
            InitializeComponent();
        }
    }
}
