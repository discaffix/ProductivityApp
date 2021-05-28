
using ProductivityApp.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.App.Views
{
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; } = new ProfileViewModel();

        public ProfilePage()
        {
            InitializeComponent();
        }
    }
}
