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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GalaSoft.MvvmLight.Command;
using ProductivityApp.AppTesting.Helpers;

namespace ProductivityApp.AppTesting.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // NOTE: This class does not have generic properties, but it will be added in the future to make the code more readable

        public ICommand StartSession;
        public ICommand StopSession;
        public ICommand SearchFieldEnterCommand;
        public ICommand TextChangedCommand;
        public ICommand OpenClosePaneCommand;

        public ICommand SaveChangesCommand;
        // text
        private string _sessionDescription = string.Empty;
        private string _elapsedTime = string.Empty;
        private string _projectSearchField = string.Empty;

        // buttons
        private bool _startSessionBtnEnabled = true;
        private bool _openPaneBtnEnabled;
        
        private bool _stopSessionBtnEnabled;

        private int _returnedProjectId = 0;
        private Session _session = new Session();

        // collections 
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        private ObservableCollection<Project> _queriedProjects = new ObservableCollection<Project>();
        private ObservableCollection<Session> _sessions = new ObservableCollection<Session>();

        private readonly CrudOperations _dataAccess = new CrudOperations();

        private Session _selectedSession;

        public MainViewModel()

        {
            var timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            OpenClosePaneCommand = new Helpers.RelayCommand<string>(openClose =>
            {
                OpenPaneBtnEnabled = !OpenPaneBtnEnabled;
            });


            TextChangedCommand = new Helpers.RelayCommand<AutoSuggestBoxTextChangedEventArgs>(args =>
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
            });

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
                _session.ProjectId = 1;

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
                    SessionDescription = string.Empty;

                    _session = new Session();

                    await LoadSessionsAsync();
                }
            });

            // search
            SearchFieldEnterCommand = new Helpers.RelayCommand<KeyRoutedEventArgs>(async searchFieldEnter =>
           {
                // When the enter key is pressed in the search field

                if (searchFieldEnter.Key != VirtualKey.Enter) return;
               if (string.IsNullOrWhiteSpace(ProjectSearchField) || ProjectSearchField.Length < 3) return;

                // create a new project object
                var project = new Project() { ProjectName = ProjectSearchField };

                // TODO: do something with returnedProjectId
                project.ProjectName = ProjectSearchField;
               var success = int.TryParse(await _dataAccess.AddEntryToDatabase("projects", project), out _returnedProjectId);

                // reload project if a value has been returned and parsed
                if (success)
                   await LoadProjectsASync();
           });


            SaveChangesCommand = new Helpers.RelayCommand<bool>(async _ =>
            {
                await _dataAccess.UpdateDatabaseEntry("sessions", SelectedSession);
            });
        }

        private void Timer_Tick(object sender, object e)
        {
            ElapsedTime = !StartSessionBtnEnabled ? (DateTime.Now - _session.StartTime).ToString(@"hh\:mm\:ss") : string.Empty;
        }

        internal async Task LoadSessionsAsync()
        {
            Sessions = new ObservableCollection<Session>();

            var sessions = await _dataAccess.GetDataFromUri<Session>("sessions");

            foreach (var session in sessions)
                if (!string.IsNullOrWhiteSpace(session.Description))
                    Sessions.Add(session);

            Sessions = new ObservableCollection<Session>(Sessions.OrderByDescending(d => d.StartTime));
        }

        internal async Task LoadProjectsASync()
        {
            Projects = new ObservableCollection<Project>();

            var projects = await _dataAccess.GetDataFromUri<Project>("projects");
            foreach (var project in projects)
                if (!string.IsNullOrWhiteSpace(project.ProjectName))
                    Projects.Add(project);

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

        public bool OpenPaneBtnEnabled
        {
            get => _openPaneBtnEnabled;
            set
            {
                if (Equals(_openPaneBtnEnabled, value)) return;
                _openPaneBtnEnabled = value;
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
            }
        }

        public ObservableCollection<Session> Sessions
        {
            get => _sessions;
            set
            {
                if (Equals(_sessions, value)) return;
                _sessions = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set
            {
                if (Equals(_projects, value)) return;
                _projects = value;
                RaisePropertyChanged();
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

        public Session SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (Equals(_selectedSession, value)) return;
                _selectedSession = value;
                RaisePropertyChanged();
            }
        }
    }
}
