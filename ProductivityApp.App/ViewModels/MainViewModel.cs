using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using ProductivityApp.App.DataAccess;
using ProductivityApp.App.Helpers;
using ProductivityApp.Model;
using ObservableObject = Microsoft.Toolkit.Mvvm.ComponentModel.ObservableObject;

namespace ProductivityApp.App.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        // NOTE: This class does not have generic properties, but it will be added in the future to make the code more readable

        public ICommand StartSessionCommand;
        public ICommand StopSessionCommand;
        public ICommand SearchFieldEnterCommand;
        public ICommand TextChangedCommand;
        public ICommand OpenClosePaneCommand;
        public ICommand SaveChangesCommand;
        public ICommand PageLoadedCommand;
        public ICommand DeleteEntryCommand;
        // text
        private string _sessionDescription = string.Empty;
        private string _elapsedTime = string.Empty;
        private string _projectSearchField = string.Empty;

        // buttons
        private bool _startSessionBtnEnabled = true;
        private bool _openPaneBtnEnabled;

        private bool _stopSessionBtnEnabled;

        private Session _session = new Session();

        // collections 
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        private ObservableCollection<Project> _queriedProjects = new ObservableCollection<Project>();
        private ObservableCollection<Session> _sessions = new ObservableCollection<Session>();

        private ObservableCollection<GroupInfosList> _groupedSessions = new ObservableCollection<GroupInfosList>();

        private Session _selectedSession;

        private readonly CrudOperations _dataAccess = new CrudOperations("http://localhost:60098/api", new HttpClient());

        public MainViewModel()

        {
            var timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            OpenClosePaneCommand = new RelayCommand(ChangePanelState);
            TextChangedCommand = new RelayCommand(TextChangedSuggestBox);
            StartSessionCommand = new RelayCommand(StartSession);
            StopSessionCommand = new RelayCommand(StopSession);
            SearchFieldEnterCommand = new RelayCommand<KeyRoutedEventArgs>(QuerySearchFieldProjectNames);
            SaveChangesCommand = new RelayCommand(SaveChangesToDatabaseEntry);
            DeleteEntryCommand = new RelayCommand(DeleteDatabaseEntry);
            PageLoadedCommand = new RelayCommand(PageLoaded);
        }

        public async void PageLoaded()
        {
            await LoadProjectsASync();
            await LoadSessionsAsync();
        }

        public async void DeleteDatabaseEntry()
        {
            var success = await _dataAccess.DeleteDatabaseEntry(SelectedSession);

            if (!success) return;

            // https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/adaptive-interactive-toasts?tabs=builder-syntax

            new ToastContentBuilder()
                .AddText("Session Deleted", hintMaxLines: 1)
                .AddText($"Id: {SelectedSession.SessionId}")
                .AddText(DateTime.Now.ToShortTimeString())
                .Show();

            await LoadSessionsAsync();
            SelectedSession = null;
        }

        public async void QuerySearchFieldProjectNames(KeyRoutedEventArgs searchFieldEnter)
        {
            // When the enter key is pressed in the search field

            if (searchFieldEnter.Key != VirtualKey.Enter) return;
            if (string.IsNullOrWhiteSpace(ProjectSearchField) || ProjectSearchField.Length < 3) return;

            // create a new project object
            var project = new Project() { ProjectName = ProjectSearchField };

            // TODO: do something with returnedProjectId
            project.ProjectName = ProjectSearchField;
            var success = await _dataAccess.AddEntryToDatabase(project);

            // reload project if a value has been returned and parsed
            if (success)
                await LoadProjectsASync();
        }

        public async void SaveChangesToDatabaseEntry()
        {
            var success = await _dataAccess.UpdateDatabaseEntry(SelectedSession);

            if (success)
                await LoadSessionsAsync();


        }

        /// <summary>
        /// Start tracking a session
        /// </summary>
        public void StartSession()
        {
            _session.Description = _sessionDescription;
            _session.StartTime = DateTime.Now;

            StartSessionBtnEnabled = false;
            StopSessionBtnEnabled = true;
        }

        /// <summary>
        /// Stops tracking the session, and saves the session object in question in the database
        /// </summary>
        public async void StopSession()
        {
            StopSessionBtnEnabled = false;
            _session.EndTime = DateTime.Now;
            _session.UserId = 1;
            _session.ProjectId = 1;

            try
            {
                await _dataAccess.AddEntryToDatabase(_session);
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }
            finally
            {
                StartSessionBtnEnabled = true;
                SessionDescription = string.Empty;

                _session = new Session();

                await LoadSessionsAsync();
            }
        }


        /// <summary>
        /// Texts the changed suggest box.
        /// </summary>
        public void TextChangedSuggestBox()
        {
            var selectedItems = new ObservableCollection<Project>();
            var text = ProjectSearchField.ToLower().Split(" ");

            foreach (var project in Projects)
            {
                var foundProjects = text.All((key) => project.ProjectName.ToLower().Contains(key));

                if (foundProjects)
                    selectedItems.Add(project);
            }

            if (selectedItems.Count == 0)
                selectedItems.Add(new Project() { ProjectName = "No Project Found" });

            QueriedProjects = selectedItems;
        }

        public void ChangePanelState()
        {
            OpenPaneBtnEnabled = !OpenPaneBtnEnabled;
        }

        private void Timer_Tick(object sender, object e)
        {
            ElapsedTime = !StartSessionBtnEnabled ? (DateTime.Now - _session.StartTime).ToString(@"hh\:mm\:ss") : string.Empty;
        }

        internal async Task LoadSessionsAsync()
        {
            Sessions = new ObservableCollection<Session>();
            GroupedSessions = new ObservableCollection<GroupInfosList>();

            var sessions = await _dataAccess.GetDataFromUri<Session>("sessions");

            foreach (var session in sessions)
                if (!string.IsNullOrWhiteSpace(session.Description))
                    Sessions.Add(session);

            Sessions = new ObservableCollection<Session>(Sessions.OrderByDescending(d => d.StartTime));

            var query = from session in Sessions
                group session by session.StartTime.ToString("d")
                into g
                orderby g.Key
                select new {GroupName = g.Key, Sessions = g};

            foreach (var g in query)
            {
                var info = new GroupInfosList {Key = g.GroupName};
                info.AddRange(g.Sessions);
                GroupedSessions.Add(info);
            }

            GroupedSessions = new ObservableCollection<GroupInfosList>(GroupedSessions.OrderByDescending(k => k.Key is DateTime key ? key : default));
        }

        internal async Task LoadProjectsASync()
        {
            Projects = new ObservableCollection<Project>();
            var projects = await _dataAccess.GetDataFromUri<Project>("projects");
            foreach (var project in projects)
                if (!string.IsNullOrWhiteSpace(project.ProjectName))
                    Projects.Add(project);
        }

        public ObservableCollection<GroupInfosList> GroupedSessions
        {
            get => _groupedSessions;
            set => SetProperty(ref _groupedSessions, value);
        }

        public string ElapsedTime
        {
            get => _elapsedTime;
            set => SetProperty(ref _elapsedTime, value);
        }

        public bool StartSessionBtnEnabled
        {
            get => _startSessionBtnEnabled;
            set => SetProperty(ref _startSessionBtnEnabled, value);
        }

        public bool OpenPaneBtnEnabled
        {
            get => _openPaneBtnEnabled;
            set => SetProperty(ref _openPaneBtnEnabled, value);
        }

        public bool StopSessionBtnEnabled
        {
            get => _stopSessionBtnEnabled;
            set => SetProperty(ref _stopSessionBtnEnabled, value);
        }

        public string SessionDescription
        {
            get => _sessionDescription;
            set => SetProperty(ref _sessionDescription, value);
        }

        public string ProjectSearchField
        {
            get => _projectSearchField;
            set => SetProperty(ref _projectSearchField, value);
        }

        public ObservableCollection<Session> Sessions
        {
            get => _sessions;
            set => SetProperty(ref _sessions, value);
        }

        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => SetProperty(ref _projects, value);
        }

        public ObservableCollection<Project> QueriedProjects
        {
            get => _queriedProjects;
            set => SetProperty(ref _queriedProjects, value);
        }

        public Session SelectedSession
        {
            get => _selectedSession;
            set => SetProperty(ref _selectedSession, value);
        }
    }
}
