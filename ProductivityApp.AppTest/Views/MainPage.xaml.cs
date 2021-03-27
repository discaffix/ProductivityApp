using System;

using ProductivityApp.AppTest.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.AppTest.Views
{
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.LoadSessionsAsync();
        }
    }
}
