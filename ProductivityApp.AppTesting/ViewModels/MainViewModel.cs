using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ProductivityApp.Model;
using ProductivityApp.AppTesting.DataAccess;
using ProductivityApp.AppTesting.Helpers;
using System.Windows.Input;
using System;
using System.Diagnostics;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand StartSession, StopSession;
        private string _sessionDescription = "";
        private Session session = new Session();
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

        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();

        private readonly CrudOperations _dataAccess = new Sessions();

        public MainViewModel()
        {
            StartSession = new RelayCommand<string>(register =>
            {
                session.Description = _sessionDescription;
                session.StartTime = DateTime.Now;
            });

            StopSession = new RelayCommand<string>(async login =>
            {
                session.EndTime = DateTime.Now;

                if (await _dataAccess.AddEntryToDatabase<Session>("sessions", session)) {
                    Debug.Print("Session created");
                }
                else
                {
                    Debug.Print("Session failed");
                }
                //MenuNavigationHelper.UpdateView(typeof(RegisterViewModel).FullName);
            });
        }

        //internal async Task LoadSessionsAsync()
        //{
        //    MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);

        //    var sessions = await _sessionsDataAccess.GetSessionsAsync();
        //    foreach (var session in sessions)
        //        Sessions.Add(session);
        //}

        public void RedirectLoginPage()
        {
            MenuNavigationHelper.UpdateView(typeof(SignInViewModel).FullName);
        }
    }
}
