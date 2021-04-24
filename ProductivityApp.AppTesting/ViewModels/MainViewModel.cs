using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ProductivityApp.Model;
using System.Threading.Tasks;
using ProductivityApp.Apptesting.DataAccess;
using ProductivityApp.AppTesting.Helpers;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _sessionDescription = "test";
        public string SessionDescription
        {
            get => _sessionDescription;
            set
            {
                if (!string.Equals(_sessionDescription, value))
                {
                    _sessionDescription = value;
                    RaisePropertyChanged();
                }
            }
        }



        /// <summary>
        /// Gets or sets the sessions.
        /// </summary>
        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();

        private readonly Sessions _sessionsDataAccess = new Sessions();

        public MainViewModel()
        {

        }

        internal async Task LoadSessionsAsync()
        {
            MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);

            var sessions = await _sessionsDataAccess.GetSessionsAsync();
            foreach (var session in sessions)
                Sessions.Add(session);
        }

        public void RedirectLoginPage()
        {
            MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);
        }
    }
}
