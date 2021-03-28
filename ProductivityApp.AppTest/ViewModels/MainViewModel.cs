using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ProductivityApp.AppTest.DataAccess;
using ProductivityApp.Model;

namespace ProductivityApp.AppTest.ViewModels
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
            var sessions = await sessionsDataAccess.GetSessionsAsync();
            foreach (Session session in sessions)
                Sessions.Add(session);
        }
    }
}
