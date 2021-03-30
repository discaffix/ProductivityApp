using System;

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
    }
}
