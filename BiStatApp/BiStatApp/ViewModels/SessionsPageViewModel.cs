using BiStatApp.Models;
using BiStatApp.Views;
using BiStatApp.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using MvvmHelpers;
using System.Data;

namespace BiStatApp.ViewModels
{
    public class SessionsPageViewModel : BaseViewModel
    {
        private SessionViewModel _selectedSession;
        private readonly ISessionStore _sessionStore;
        private readonly IPageService _pageService;

        //private bool _isDataLoaded = false;

        public ObservableRangeCollection<SessionViewModel> Sessions { get; private set; }
            = new ObservableRangeCollection<SessionViewModel>();
        public ObservableRangeCollection<SessionViewModel> AllSessions { get; private set; }
            = new ObservableRangeCollection<SessionViewModel>();

        public ObservableRangeCollection<string> FilterOptions { get; }

        public ObservableRangeCollection<string> FilterPeriodOptions { get; }

        string _selectedPeriodFilter = "All";

        public string SelectedPeriodFilter
        {
            get => _selectedPeriodFilter;
            set
            {
                SetValue(ref _selectedPeriodFilter, value);
                OnPropertyChanged("SelectedPeriodFilter");
                FilterSessions();
            }
        }

        string _selectedFilter = "All";

        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                SetValue(ref _selectedFilter, value);
                OnPropertyChanged("SelectedFilter");
                FilterSessions();
            }
        }

        public SessionViewModel SelectedSession
        {
            get { return _selectedSession; }
            set { SetValue(ref _selectedSession, value); }
        }

        public double OverallHitPercentage
        {
            get
            {
                double hits = 0;
                foreach(var s in Sessions)
                {
                    hits += s.HitPercentage;
                }
                return hits / Sessions.Count();
            }
        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand AddSessionCommand { get; private set; }
        public ICommand SelectSessionCommand { get; private set; }
        public ICommand DeleteSessionCommand { get; private set; }

        public ICommand ReportCommand { get; private set; }

        public SessionsPageViewModel()
        {

        }

        public SessionsPageViewModel(ISessionStore sessionStore, IPageService pageService)
        {
            _sessionStore = sessionStore;
            _pageService = pageService;

            LoadDataCommand = new Command(async () => await LoadData());
            AddSessionCommand = new Command(async () => await AddSession());
            SelectSessionCommand = new Command<SessionViewModel>(async c => await SelectSession(c));
            DeleteSessionCommand = new Command<SessionViewModel>(async c => await DeleteSession(c));
            ReportCommand = new Command(async () => await ShowReport());

            MessagingCenter.Subscribe<SessionDetailViewModel, Session>
                (this, Events.SessionAdded, OnSessionAdded);

            MessagingCenter.Subscribe<SessionDetailViewModel, Session>
                (this, Events.SessionUpdated, OnSessionUpdated);

            FilterOptions = new ObservableRangeCollection<string>
            { 
                "All",
                "1 Shot Setup",
                "5 Across",
                "Combo",
                "Time Trail",
                "Open Training",
                "Race",
                "Dry Fire"
            };

            FilterPeriodOptions = new ObservableRangeCollection<string>
            {
                "All",
                "3 Months",
                "1 Month"
            };

        }

        private void OnSessionAdded(SessionDetailViewModel source, Session session)
        {
            Sessions.Add(new SessionViewModel(session));
        }

        private void OnSessionUpdated(SessionDetailViewModel source, Session session)
        {
            var sessionInList = Sessions.SingleOrDefault(c => c.Id == session.Id);

            if (sessionInList == null)
            {
                Sessions.Add(new SessionViewModel(session));
            }
            else
            {
                sessionInList.Id = session.Id;
                sessionInList.Name = session.Name;
                sessionInList.Description = session.Description;
                sessionInList.DateTime = session.DateTime;
            }
        }

        private async Task LoadData()
        {
            //if (_isDataLoaded)
            //    return;

            //_isDataLoaded = true;
            AllSessions.Clear();
            var sessions = await _sessionStore.GetSessionsAsync();
            foreach (var session in sessions)
                AllSessions.Add(new SessionViewModel(session));
            FilterSessions();
        }

        private async Task AddSession()
        {
            await _pageService.PushAsync(new SessionDetailPage(new SessionViewModel()));
        }

        private async Task SelectSession(SessionViewModel session)
        {
            if (session == null)
                return;

            SelectedSession = null;
            await _pageService.PushAsync(new SessionDetailPage(session));
        }

        private async Task DeleteSession(SessionViewModel sessionViewModel)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {sessionViewModel.Name}?", "Yes", "No"))
            {
                Sessions.Remove(sessionViewModel);

                var session = await _sessionStore.GetSession(sessionViewModel.Id);
                await _sessionStore.DeleteSession(session);
            }
        }

        private async Task ShowReport()
        {
            await _pageService.PushAsync(new ReportPage(Sessions));
        }


        /// <summary>
        /// Filter on practice and period
        /// </summary>
        void FilterSessions()
        {
            ObservableRangeCollection<SessionViewModel> typeSessions = new ObservableRangeCollection<SessionViewModel>();
            typeSessions.ReplaceRange(AllSessions.Where(a => a.Name.Contains(SelectedFilter) || SelectedFilter == "All" ));

            ObservableRangeCollection<SessionViewModel> typePeriodSession = new ObservableRangeCollection<SessionViewModel>();

            DateTime startPeriod = new DateTime(1970, 1, 1);
            if (SelectedPeriodFilter == "3 Months")
            {
                startPeriod = DateTime.Today.AddMonths(-3);
            }
            else if (SelectedPeriodFilter == "1 Month")
            {
                startPeriod = DateTime.Today.AddMonths(-1);
            }

            typePeriodSession.ReplaceRange(typeSessions.Where(a => a.DateTime > startPeriod));

            Sessions.ReplaceRange(typePeriodSession);
        }
    }
}
