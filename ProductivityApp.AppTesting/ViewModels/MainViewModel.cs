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
        private string _elapsedTime = string.Empty;

        private bool _startSessionBtnEnabled = true;
        private bool _stopSessionBtnEnabled;
        
        private Session _session = new Session();

        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();
        private readonly CrudOperations _dataAccess = new CrudOperations();

        public MainViewModel()
        {
            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            StartSession = new RelayCommand<string>(register =>
            {
                _session.Description = _sessionDescription;
                _session.StartTime = DateTime.Now;

                StartSessionBtnEnabled = false;
                StopSessionBtnEnabled = true;
            });

            StopSession = new RelayCommand<string>(async login =>
            {
                StopSessionBtnEnabled = false;
                _session.EndTime = DateTime.Now;

                var user = await _dataAccess.GetEntryFromDatabase<User>("users", 1);
                _session.User = user;

                try
                {
                    // TODO: Doesn't save when user is set to session
                    // https://stackoverflow.com/questions/50307633/how-do-i-postasync-with-multiple-simple-types
                    await _dataAccess.AddEntryToDatabase("sessions", _session);
                }
                catch (Exception e)
                {
                    Debug.Write(e);
                }
                finally
                {
                    StartSessionBtnEnabled = true;
                    _session = new Session();
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
            ElapsedTime = !StartSessionBtnEnabled ? (DateTime.Now - _session.StartTime).ToString(@"hh\:mm\:ss") : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async Task LoadSessionsAsync()
        {
            var sessions = await _dataAccess.GetDataFromUri<Session>("sessions");

            foreach (var session in sessions)
                if (!string.IsNullOrEmpty(session.Description))
                    Sessions.Add(session);
        }

        public string ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if (Equals(_elapsedTime, value)) return;


                _elapsedTime = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        public bool StopSessionBtnEnabled
        {
            get => _stopSessionBtnEnabled;
            set
            {
                if (Equals(_stopSessionBtnEnabled, value)) return;

                _stopSessionBtnEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string SessionDescription
        {
            get => _sessionDescription;
            set
            {
                if (string.Equals(_sessionDescription, value)) return;

                _sessionDescription = value;
                RaisePropertyChanged();
            }
        }
    }
}
