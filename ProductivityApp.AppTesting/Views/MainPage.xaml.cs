using System;
using System.Threading.Tasks;
using ProductivityApp.AppTesting.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.AppTesting.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Page_LoadedAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadSessionsAsync();
        }
    }
}
