
using ProductivityApp.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.App.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterViewModel ViewModel { get; } = new RegisterViewModel();

        public RegisterPage()
        {
            InitializeComponent();
        }
    }
}
