using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ProductivityApp.Model;
using System.Threading.Tasks;
using ProductivityApp.Apptesting.DataAccess;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Views;
using ProductivityApp.AppTesting.Helpers;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string sessionDescription = "test";
        public string SessionDescription
        {
            get
            {
                return sessionDescription;
            }
            set
            {
                if (!string.Equals(sessionDescription, value))
                {
                    sessionDescription = value;
                    RaisePropertyChanged();
                }
            }
        }



        /// <summary>
        /// Gets or sets the sessions.
        /// </summary>
        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();

        private Sessions sessionsDataAccess = new Sessions();

        public MainViewModel()
        {

        }

        internal async Task LoadSessionsAsync()
        {
            MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);

            var sessions = await sessionsDataAccess.GetSessionsAsync();
            foreach (Session session in sessions)
                Sessions.Add(session);
        }

        public void RedirectLoginPage()
        {
            MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);
        }
    }
}
