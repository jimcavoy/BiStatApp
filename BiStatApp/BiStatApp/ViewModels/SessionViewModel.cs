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
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                SetValue(ref _dateTime, value);
                OnPropertyChanged(nameof(DateTime));
            }
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
            _name = session.Name;
            _description = session.Description;
            _shootingBouts = session.Bouts;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetValue(ref _name, value);
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                SetValue(ref _description, value);
                OnPropertyChanged(nameof(Description));
            }
        }

        private List<ShootingBout> _shootingBouts = new List<ShootingBout>();

        public IEnumerable<ShootingBout> Bouts
        {
            get { return _shootingBouts; }
            set
            {
                SetValue(ref _shootingBouts, value.ToList());
                OnPropertyChanged(nameof(Bouts));
            }
        }

        public double HitPercentage
        {
            get
            {
                double hits = 0;
                foreach(var sb in Bouts)
                {
                    var vm = new ShootingBoutViewModel(sb);
                    hits += 5 - vm.Misses;
                }
                return (double)(hits / (Bouts.Count() * 5));
            }
        }

    }
}
