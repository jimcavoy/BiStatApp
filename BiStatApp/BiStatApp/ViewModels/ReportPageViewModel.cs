using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class ReportPageViewModel : BaseViewModel
    {
        public List<SessionViewModel> Sessions { get; set; }

        public ReportPageViewModel(IEnumerable<SessionViewModel> sessionStore)
        {
            Sessions = sessionStore.ToList();

            LoadData();
        }

        private void LoadData()
        {
            double hits = 0;
            double proneAve = 0;
            double standingAve = 0;

            foreach (var s in Sessions)
            {
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
            OverallAverage = hits / Sessions.Count;
            ProneAverage = proneAve / (ProneShots / 5);
            StandingAverage = standingAve / (StandingShots / 5);
        }

        public double OverallAverage { get; private set; } = 0;

        public double ProneAverage { get; private set; } = 0;

        public double StandingAverage { get; private set; } = 0;

        public int TotalShots { get; private set; } = 0;

        public int ProneShots { get; private set; } = 0;

        public int StandingShots { get; private set; } = 0;

    }
}
