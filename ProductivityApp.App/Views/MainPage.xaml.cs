using System;

using ProductivityApp.App.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ProductivityApp.App.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

    }
}
