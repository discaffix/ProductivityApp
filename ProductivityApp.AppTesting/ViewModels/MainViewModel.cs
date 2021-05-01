using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ProductivityApp.Model;
using ProductivityApp.AppTesting.DataAccess;
using System.Windows.Input;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Input;
using GalaSoft.MvvmLight.Command;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // NOTE: This class does not have generic properties, but it will be added in the future to make the code more readable

        public ICommand StartSession, StopSession, SearchFieldEnterCommand;

        private string _sessionDescription = string.Empty;
        private string _elapsedTime = string.Empty;

        private bool _startSessionBtnEnabled = true;
        private bool _stopSessionBtnEnabled;

        private string _projectSearchField = string.Empty;


        private Session _session = new Session();

        public ObservableCollection<Session> Sessions { get; set; } = new ObservableCollection<Session>();
        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        private ObservableCollection<Project> _queriedProjects = new ObservableCollection<Project>();

        private readonly CrudOperations _dataAccess = new CrudOperations();

        public MainViewModel()
        {
            var timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            StartSession = new Helpers.RelayCommand<string>(register =>
            {
                _session.Description = _sessionDescription;
                _session.StartTime = DateTime.Now;

                StartSessionBtnEnabled = false;
                StopSessionBtnEnabled = true;
            });

            StopSession = new Helpers.RelayCommand<string>(async login =>
            {
                StopSessionBtnEnabled = false;
                _session.EndTime = DateTime.Now;
                _session.UserId = 1;
                _session.ProjectId = 5;

                try
                {
                    await _dataAccess.AddEntryToDatabase("Sessions", _session);
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

            SearchFieldEnterCommand = new Helpers.RelayCommand<KeyRoutedEventArgs>( async searchFieldEnter =>
            {
                // When the enter key is pressed in the search field
                int returnedProjectId;

                if (searchFieldEnter.Key == VirtualKey.Enter)
                {
                    if (string.IsNullOrWhiteSpace(ProjectSearchField) || ProjectSearchField.Length < 3) return;

                    // create a new project object
                    var project = new Project() {ProjectName = ProjectSearchField};

                    // TODO: do something with returnedProjectId
                    project.ProjectName = ProjectSearchField;
                    var success = int.TryParse(await _dataAccess.AddEntryToDatabase("projects", project), out returnedProjectId);

                    // reload project if a value has been returned and parsed
                    if (success)
                        await LoadProjectsASync();
                }
            });
        }

        private void Timer_Tick(object sender, object e)
        {
            ElapsedTime = !StartSessionBtnEnabled ? (DateTime.Now - _session.StartTime).ToString(@"hh\:mm\:ss") : string.Empty;
        }

        internal async Task LoadSessionsAsync()
        {
            var sessions = await _dataAccess.GetDataFromUri<Session>("sessions");

            foreach (var session in sessions)
                if (!string.IsNullOrWhiteSpace(session.Description))
                    Sessions.Add(session);
        }

        internal async Task LoadProjectsASync()
        {
            Projects = new ObservableCollection<Project>();

            var projects = await _dataAccess.GetDataFromUri<Project>("projects");
            foreach (var project in projects)
                if (!string.IsNullOrWhiteSpace(project.ProjectName))
                    Projects.Add(project);


            Search();
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

        public string ProjectSearchField
        {
            get => _projectSearchField;
            set
            {
                if (Equals(_projectSearchField, value)) return;

                _projectSearchField = value;
                RaisePropertyChanged();
                Search();
            }
        }

        public ObservableCollection<Project> QueriedProjects
        {
            get => _queriedProjects;
            set
            {
                if (Equals(_queriedProjects, value)) return;

                _queriedProjects = value;
                RaisePropertyChanged();
            }
        }

        internal void Search()
        {
            QueriedProjects = !string.IsNullOrEmpty(ProjectSearchField) ?
                new ObservableCollection<Project>(Projects.Where(d => d.ProjectName.ToLower().Contains(ProjectSearchField.ToLower()))) : new ObservableCollection<Project>();
        }
    }
}
