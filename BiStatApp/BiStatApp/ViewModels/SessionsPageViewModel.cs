using BiStatApp.Models;
using BiStatApp.Views;
using BiStatApp.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                FilterSessions();
            }
        }

        public SessionViewModel SelectedSession
        {
            get => _selectedSession;
            set 
            { 
                SetValue(ref _selectedSession, value);
                OnSelectSession(value);
            }
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
        public ICommand DeleteSessionCommand { get; private set; }
        public ICommand ReportCommand { get; private set; }

        public SessionsPageViewModel()
        {
            LoadDataCommand = new Command(async () => await LoadData());
            DeleteSessionCommand = new Command<SessionViewModel>(async c => await DeleteSession(c));
            ReportCommand = new Command(async () => await ShowReport());

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

            Title = "History";
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
            IsBusy = true;

            try
            {
                AllSessions.Clear();
                var sessions = await DataStore.GetSessionsAsync();
                foreach (var session in sessions)
                    AllSessions.Add(new SessionViewModel(session));
                FilterSessions();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            SelectedSession = null;
        }


        private async void OnSelectSession(SessionViewModel session)
        {
            if (session == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(SessionDetailPage)}?{nameof(SessionDetailViewModel.SessionId)}={session.Id}");
        }

        private async Task DeleteSession(SessionViewModel sessionViewModel)
        {
            if (await Application.Current.MainPage.DisplayAlert("Warning", $"Are you sure you want to delete {sessionViewModel.Name}?", "Yes", "No"))
            {
                Sessions.Remove(sessionViewModel);

                var session = await DataStore.GetSession(sessionViewModel.Id);
                await DataStore.DeleteSession(session);
            }
        }

        private async Task ShowReport()
        {
            string filter = SelectedFilter + "," + SelectedPeriodFilter;
            await Shell.Current.GoToAsync($"{nameof(ReportPage)}?{nameof(ReportPageViewModel.Filter)}={filter}");
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
