using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace BiStatApp.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        public int Id { get; set; }

        private DateTime _dateTime;
        private string _name = "";
        private string _description = "";
        private List<ShootingBout> _shootingBouts = new List<ShootingBout>();

        public DateTime DateTime
        {
            get => _dateTime;
            set => SetValue(ref _dateTime, value, "DateTime");
        }

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value, "Name");
        }

        public string Description
        {
            get => _description;
            set => SetValue(ref _description, value, "Description");
        }

        public IEnumerable<ShootingBout> Bouts
        {
            get => _shootingBouts;
            set => SetValue(ref _shootingBouts, value.ToList(), "Bouts");
        }

        public SessionViewModel()
        {
            DateTime = System.DateTime.Now;
            var initialBout = new ShootingBout()
            {
                Position = ShootingBout.PositionEnum.PRONE,
                Alpha = false,
                Bravo = false,
                Charlie = false,
                Delta = false,
                Echo = false
            };

            _shootingBouts.Add(initialBout);
        }

        public SessionViewModel(Session session)
        {
            Id = session.Id;
            DateTime = session.DateTime;
            _name = session.Name ?? "";
            _description = session.Description ?? "";
            _shootingBouts = session.Bouts;
        }

        public double HitPercentage
        {
            get
            {
                double hits = 0;
                if (Bouts.Count() == 0)
                {
                    return hits;
                }

                foreach (var sb in Bouts)
                {
                    var vm = new ShootingBoutViewModel(sb);
                    hits += 5 - vm.Misses;
                }
                return (double)(hits / (Bouts.Count() * 5));
            }
        }

        public int ProneShots
        {
            get
            {
                int shots = 0;
                foreach (var sb in Bouts)
                {
                    if (sb.Position == ShootingBout.PositionEnum.PRONE)
                    {
                        shots += 5;
                    }
                }
                return shots;
            }
        }

        public int StandingShots
        {
            get
            {
                int shots = 0;
                foreach (var sb in Bouts)
                {
                    if (sb.Position == ShootingBout.PositionEnum.STANDING)
                    {
                        shots += 5;
                    }
                }
                return shots;
            }
        }

        public double ProneAverage
        {
            get
            {
                double hits = 0.0;
                foreach (var sb in Bouts)
                {
                    if (sb.Position == ShootingBout.PositionEnum.PRONE)
                    {
                        var vm = new ShootingBoutViewModel(sb);
                        hits += 5 - vm.Misses;
                    }
                }
                return (double)(hits / ProneShots);
            }
        }

        public double StandingAverage
        {
            get
            {
                double hits = 0.0;
                foreach (var sb in Bouts)
                {
                    if (sb.Position == ShootingBout.PositionEnum.STANDING)
                    {
                        var vm = new ShootingBoutViewModel(sb);
                        hits += 5 - vm.Misses;
                    }
                }
                return (double)(hits / StandingShots);
            }
        }
    }
}
