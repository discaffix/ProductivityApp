using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ProductivityApp.Model;
using ProductivityApp.AppTesting.DataAccess;
using ProductivityApp.AppTesting.Helpers;
using System.Windows.Input;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand StartSession, StopSession;

        private string _sessionDescription = string.Empty;
        private bool _startSessionBtnEnabled = true;
        private bool _stopSessionBtnEnabled = false;
        private string _elapsedTime = string.Empty;
        private readonly DispatcherTimer timer;
        private Session session = new Session();

        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();
        private readonly CrudOperations _dataAccess = new CrudOperations();

        public MainViewModel()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            StartSession = new RelayCommand<string>(register =>
            {
                session.Description = _sessionDescription;
                session.StartTime = DateTime.Now;

                StartSessionBtnEnabled = false;
                StopSessionBtnEnabled = true;
            });

            StopSession = new RelayCommand<string>(async login =>
            {
                StopSessionBtnEnabled = false;
                session.EndTime = DateTime.Now;

                User user = await _dataAccess.GetEntryFromDatabase<User>("users", 1);
                session.User = user;

                try
                {
                    // TODO: Doesn't save when user is set to session
                    // https://stackoverflow.com/questions/50307633/how-do-i-postasync-with-multiple-simple-types
                    await _dataAccess.AddEntryToDatabase<Session>("sessions", session);
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
                finally
                {
                    StartSessionBtnEnabled = true;
                    session = new Session();
                    SessionDescription = string.Empty;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, object e)
        {
            if (session.StartTime != null && !StartSessionBtnEnabled)
                ElapsedTime = (DateTime.Now - session.StartTime).ToString(@"hh\:mm\:ss");
            else
                ElapsedTime = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async Task LoadSessionsAsync()
        {
            var sessions = await _dataAccess.GetDataFromUri<Session>("sessions");

            foreach (Session session in sessions)
                if (session.Description != null && session.Description.Length > 0)
                    Sessions.Add(session);
        }

        public void RedirectLoginPage()
        {
            MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);
        }

        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (Equals(_elapsedTime, value)) return;


                _elapsedTime = value;
                RaisePropertyChanged("ElapsedTime");
            }
        }

        /// <summary>
        /// getter and setters for the property responsible for enabling the start session button
        /// </summary>
        public bool StartSessionBtnEnabled
        {
            get => _startSessionBtnEnabled;
            set
            {
                if (Equals(_startSessionBtnEnabled, value)) return;

                _startSessionBtnEnabled = value;
                RaisePropertyChanged("StartSessionBtnEnabled");
            }
        }

        public bool StopSessionBtnEnabled
        {
            get => _stopSessionBtnEnabled;
            set
            {
                if (Equals(_stopSessionBtnEnabled, value)) return;

                _stopSessionBtnEnabled = value;
                RaisePropertyChanged("StopSessionBtnEnabled");
            }
        }

        public string SessionDescription
        {
            get => _sessionDescription;
            set
            {
                if (string.Equals(_sessionDescription, value)) return;

                _sessionDescription = value;
                RaisePropertyChanged("SessionDescription");
            }
        }
    }
}
