using ProductivityApp.AppTesting.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ProductivityApp.AppTesting.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel => ViewModelLocator.Current.MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Page_LoadedAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //await ViewModel.LoadSessionsAsync();
        }
    }
}
