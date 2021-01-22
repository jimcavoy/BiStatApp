using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    [QueryProperty(nameof(Filter), nameof(Filter))]
    public class ReportPageViewModel : BaseViewModel
    {
        private string _filter;
        private double _overallAve;
        private double _proneAve;
        private double _standingAve;
        private int _totalShots;
        private int _proneShots;
        private int _standingShots;
        private int _sessionsCount;

        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                LoadData(value);
            }
        }

        public double OverallAverage
        {
            get => _overallAve;
            private set => SetValue(ref _overallAve, value);
        }

        public double ProneAverage
        {
            get => _proneAve;
            private set => SetValue(ref _proneAve, value);
        }

        public double StandingAverage
        {
            get => _standingAve;
            private set => SetValue(ref _standingAve, value);
        }

        public int TotalShots
        {
            get => _totalShots;
            private set => SetValue(ref _totalShots, value);
        }

        public int ProneShots
        {
            get => _proneShots;
            private set => SetValue(ref _proneShots, value); 
        }

        public int StandingShots
        {
            get => _standingShots;
            private set => SetValue(ref _standingShots, value);
        }

        public int SessionsCount
        {
            get => _sessionsCount;
            private set => SetValue(ref _sessionsCount, value);
        }

        public ReportPageViewModel()
        {

        }

        private async void LoadData(string typeAndPeriod)
        {
            string filter = Uri.UnescapeDataString(typeAndPeriod);
            string[] filters = filter.Split(',');
            string type = filters[0] ?? "All";
            string period = (filters.Length == 1 || filters[1] == null)? "All" : filters[1];

            var AllSessions = await DataStore.GetSessionsAsync();
            List<SessionViewModel> Sessions = new List<SessionViewModel>();

            foreach(var session in AllSessions)
            {
                Sessions.Add(new SessionViewModel(session));
            }

            ObservableRangeCollection<SessionViewModel> typeSessions = new ObservableRangeCollection<SessionViewModel>();
            typeSessions.ReplaceRange(Sessions.Where(s => s.Name.Contains(type) || type == "All" ));

            ObservableRangeCollection<SessionViewModel> typePeriodSessions = new ObservableRangeCollection<SessionViewModel>();

            DateTime startPeriod = new DateTime(1970, 1, 1);
            if (period == "3 Months")
            {
                startPeriod = DateTime.Today.AddMonths(-3);
            }
            else if (period == "1 Month")
            {
                startPeriod = DateTime.Today.AddMonths(-1);
            }

            typePeriodSessions.ReplaceRange(typeSessions.Where(a => a.DateTime > startPeriod));

            double hits = 0;
            double proneAve = 0;
            double standingAve = 0;
            int sessionCount = 0;

            foreach (var s in typePeriodSessions)
            {
                if (s.Bouts.Count() > 0)
                {
                    sessionCount++;
                    hits += s.HitPercentage;
                    TotalShots += s.Bouts.Count() * 5;
                    foreach (var b in s.Bouts)
                    {
                        if (b.Position == ShootingBout.PositionEnum.PRONE)
                        {
                            ProneShots += 5;
                            proneAve += s.ProneAverage;
                        }
                        else
                        {
                            StandingShots += 5;
                            standingAve += s.StandingAverage;
                        }
                    } 
                }
            }
            OverallAverage = hits / sessionCount;
            ProneAverage = proneAve / (ProneShots / 5);
            StandingAverage = standingAve / (StandingShots / 5);
            SessionsCount = sessionCount;
        }
    }
}
