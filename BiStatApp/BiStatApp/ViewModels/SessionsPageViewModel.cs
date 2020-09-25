using BiStatApp.Models;
using BiStatApp.Views;
using BiStatApp.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace BiStatApp.ViewModels
{
    public class SessionsPageViewModel : BaseViewModel
    {
        private SessionViewModel _selectedSession;
        private readonly ISessionStore _sessionStore;
        private readonly IPageService _pageService;

        //private bool _isDataLoaded = false;

        public ObservableCollection<SessionViewModel> Sessions { get; private set; }
            = new ObservableCollection<SessionViewModel>();

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

            MessagingCenter.Subscribe<SessionDetailViewModel, Session>
                (this, Events.SessionAdded, OnSessionAdded);

            MessagingCenter.Subscribe<SessionDetailViewModel, Session>
                (this, Events.SessionUpdated, OnSessionUpdated);
        }

        private void OnSessionAdded(SessionDetailViewModel source, Session session)
        {
            Sessions.Add(new SessionViewModel(session));
        }

        private void OnSessionUpdated(SessionDetailViewModel source, Session session)
        {
            var sessionInList = Sessions.Single(c => c.Id == session.Id);

            sessionInList.Id = session.Id;
            sessionInList.Name = session.Name;
            sessionInList.Description = session.Description;
            sessionInList.DateTime = session.DateTime;
        }

        private async Task LoadData()
        {
            //if (_isDataLoaded)
            //    return;

            //_isDataLoaded = true;
            Sessions.Clear();
            var sessions = await _sessionStore.GetSessionsAsync();
            foreach (var session in sessions)
                Sessions.Add(new SessionViewModel(session));
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
    }
}
