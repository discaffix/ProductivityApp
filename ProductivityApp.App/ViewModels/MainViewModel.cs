using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.Notifications;
using ProductivityApp.App.DataAccess;
using ProductivityApp.App.Helpers;
using ProductivityApp.App.Views;
using ProductivityApp.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ProductivityApp.App.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly CrudOperations
            _dataAccess = new CrudOperations("http://localhost:60098/api", new HttpClient());
        private ICommand _deleteEntryCommand;
        private string _elapsedTime = string.Empty;
        private ObservableCollection<GroupInfosList> _groupedSessions = new ObservableCollection<GroupInfosList>();
        private ICommand _openClosePaneCommand;
        private bool _openPaneBtnEnabled;
        private ICommand _pageLoadedCommand;

        // collections 
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        private string _projectSearchField = string.Empty;
        private ObservableCollection<Project> _queriedProjects = new ObservableCollection<Project>();
        private ICommand _querySubmittedCommand;
        private ICommand _saveChangesCommand;
        private ICommand _searchFieldEnterCommand;
        private Session _selectedSession;
        private bool _trackingState;
        private ICommand _trackingStateCommand;
        private Session _session = new Session();

        private string _sessionDescription = string.Empty;
        private ObservableCollection<Session> _sessions = new ObservableCollection<Session>();
        private ObservableCollection<SessionTag> _sessionTags = new ObservableCollection<SessionTag>();


        private bool _startSessionBtnEnabled = true;
        private ICommand _startSessionCommand;
        private bool _stopSessionBtnEnabled;
        private ICommand _stopSessionCommand;
        private ObservableCollection<Tag> _tags = new ObservableCollection<Tag>();
        private ICommand _textChangeCommand;
        private int _userId;

        public MainViewModel()
        {
            StartDispatcherTimer();
        }


        public ICommand StartSessionCommand =>
            _startSessionCommand ?? (_startSessionCommand = new RelayCommand(StartSession));

        public ICommand StopSessionCommand =>
            _stopSessionCommand ?? (_stopSessionCommand = new RelayCommand(StopSession));

        public ICommand SearchFieldEnterCommand =>
            _searchFieldEnterCommand ?? (_searchFieldEnterCommand =
                new RelayCommand<KeyRoutedEventArgs>(QuerySearchFieldProjectNames));

        public ICommand TrackingStateCommand =>
            _trackingStateCommand ?? (_trackingStateCommand = new RelayCommand(TrackingStateChanger));

        public ICommand TextChangedCommand =>
            _textChangeCommand ?? (_textChangeCommand = new RelayCommand(TextChangedSuggestBox));

        public ICommand OpenClosePaneCommand =>
            _openClosePaneCommand ?? (_openClosePaneCommand = new RelayCommand(ChangePanelState));

        public ICommand SaveChangesCommand =>
            _saveChangesCommand ?? (_saveChangesCommand = new RelayCommand(SaveChangesToDatabaseEntry));

        public ICommand PageLoadedCommand =>
            _pageLoadedCommand ?? (_pageLoadedCommand = new RelayCommand(PageLoaded));

        public ICommand DeleteEntryCommand =>
            _deleteEntryCommand ?? (_deleteEntryCommand = new RelayCommand(DeleteDatabaseEntry));

        public ICommand QuerySubmittedCommand =>
            _querySubmittedCommand ?? (_querySubmittedCommand = new RelayCommand<Project>(QuerySubmitted));

        public ObservableCollection<GroupInfosList> GroupedSessions
        {
            get => _groupedSessions;
            set => SetProperty(ref _groupedSessions, value);
        }

        public bool TrackingState
        {
            get => _trackingState;
            set => SetProperty(ref _trackingState, value);
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

        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        public ObservableCollection<SessionTag> SessionTags
        {
            get => _sessionTags;
            set => SetProperty(ref _sessionTags, value);
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

        private void StartDispatcherTimer()
        {
            var timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        /// <summary>
        /// Executed when the MainPage is loaded
        /// </summary>
        public async void PageLoaded()
        {
            AuthenticationAttempt();

            await LoadProjectsASync();
            await LoadSessionsAsync();
            await LoadSessionTagsAsync();
        }

        /// <summary>
        /// Checks whether or not a user is logged in
        /// </summary>
        public void AuthenticationAttempt()
        {
            var composite = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["user"];

            try
            {
                if (composite == null || (int)composite["id"] == 0)
                {
                    MenuNavigationHelper.UpdateView(typeof(SignInPage));
                    return;
                }

                _userId = (int)composite["id"];
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e);
            }
        }


        public async void DeleteDatabaseEntry()
        {
            var success = await _dataAccess.DeleteDatabaseEntry(SelectedSession);

            if (!success) return;

            BuildAndDisplayToast("Session Deleted", $"Id: {SelectedSession.SessionId}");

            await LoadSessionsAsync();
            SelectedSession = null;
        }

        public void BuildAndDisplayToast(string title, string firstLine)
        {

            new ToastContentBuilder()
                .AddText(title, hintMaxLines: 1)
                .AddText(firstLine)
                .AddText(DateTimeOffset.UtcNow.LocalDateTime.ToString(CultureInfo.InvariantCulture))
                .Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchFieldEnter"></param>
        public async void QuerySearchFieldProjectNames(KeyRoutedEventArgs searchFieldEnter)
        {
            // When the enter key is pressed in the search field

            if (searchFieldEnter.Key != VirtualKey.Enter) return;
            if (string.IsNullOrWhiteSpace(ProjectSearchField) || ProjectSearchField.Length < 3) return;

            // create a new project object
            var project = new Project { ProjectName = ProjectSearchField };

            // TODO: do something with returnedProjectId
            project.ProjectName = ProjectSearchField;
            var success = await _dataAccess.AddEntryToDatabase(project);

            // reload project if a value has been returned and parsed
            if (success)
                await LoadProjectsASync();
        }

        /// <summary>
        /// Assigns selected project to the selectedSession object (the one that is currently being edited)
        /// </summary>
        /// <param name="project">The project.</param>
        public void QuerySubmitted(Project project)
        {
            SelectedSession.ProjectId = project.ProjectId;
        }

        public async void SaveChangesToDatabaseEntry()
        {
            var success = await _dataAccess.UpdateDatabaseEntry(SelectedSession);
            if (success)
                await LoadSessionsAsync();
        }

        /// <summary>
        ///     Start tracking a session
        /// </summary>
        public void StartSession()
        {
            _session.Description = _sessionDescription;
            _session.StartTime = DateTime.Now;

            StartSessionBtnEnabled = false;
            StopSessionBtnEnabled = true;
            BuildAndDisplayToast("Started tracking session!", "Press the stop button to stop tracking.");
        }

        /// <summary>
        ///     Stops tracking the session, and saves the session object in question in the database
        /// </summary>
        public async void StopSession()
        {
            StopSessionBtnEnabled = false;
            _session.EndTime = DateTime.Now;
            _session.UserId = _userId;
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

                BuildAndDisplayToast("Session Saved", "");
            }


        }


        /// <summary>
        /// Handling for the Project SuggestBox
        /// </summary>
        public void TextChangedSuggestBox()
        {
            var selectedItems = new ObservableCollection<Project>();
            var text = ProjectSearchField.ToLower().Split(" ");

            foreach (var project in Projects)
            {
                var foundProjects = text.All(key => project.ProjectName.ToLower().Contains(key));

                if (foundProjects)
                    selectedItems.Add(project);
            }

            if (selectedItems.Count == 0)
                selectedItems.Add(new Project { ProjectName = "No Project Found" });

            QueriedProjects = selectedItems;
        }

        /// <summary>
        /// Sets the tracking state and starts/ends session
        /// </summary>
        private void TrackingStateChanger()
        {
            if (!TrackingState)
                StartSession();
            else
            {
                StopSession();
            }

            TrackingState = !TrackingState;
        }

        /// <summary>
        /// Changes the state of the panel.
        /// </summary>
        public void ChangePanelState()
        {
            OpenPaneBtnEnabled = !OpenPaneBtnEnabled;
        }

        /// <summary>
        /// Executed every time interval to update the timer.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Timer_Tick(object sender, object e)
        {
            ElapsedTime = !StartSessionBtnEnabled
                ? (DateTime.Now - _session.StartTime).ToString(@"hh\:mm\:ss")
                : string.Empty;
        }

        /// <summary>
        /// Loads the sessionTags asynchronous.
        /// </summary>
        internal async Task LoadSessionTagsAsync()
        {
            var sessionTags = await _dataAccess.GetDataFromUri<SessionTag>("sessionTags");

            foreach (var sessionTag in sessionTags)
                if (sessionTag.Tag.UserId == _userId)
                    SessionTags.Add(sessionTag);
        }

        /// <summary>
        /// Loads the sessions asynchronous.
        /// </summary>
        internal async Task LoadSessionsAsync()
        {
            Sessions = new ObservableCollection<Session>();
            GroupedSessions = new ObservableCollection<GroupInfosList>();

            var sessions = await _dataAccess.GetDataFromUri<Session>("sessions");


            foreach (var session in sessions)
                if (session.UserId == _userId)
                    Sessions.Add(session);

            Sessions = new ObservableCollection<Session>(Sessions.OrderByDescending(d => d.StartTime));

            var query = from session in Sessions
                        group session by session.StartTime.ToString("d")
                into g
                        orderby g.Key
                        select new { GroupName = g.Key, Sessions = g };

            foreach (var g in query)
            {
                var info = new GroupInfosList { Key = g.GroupName };
                info.AddRange(g.Sessions);
                GroupedSessions.Add(info);
            }

            GroupedSessions =
                new ObservableCollection<GroupInfosList>(
                    GroupedSessions.OrderByDescending(k => k.Key is DateTimeOffset key ? key : default));
        }

        internal async Task LoadProjectsASync()
        {
            Projects = new ObservableCollection<Project>();
            var projects = await _dataAccess.GetDataFromUri<Project>("projects");


            foreach (var project in projects)
                if (!string.IsNullOrWhiteSpace(project.ProjectName))
                    Projects.Add(project);
        }
    }
}
