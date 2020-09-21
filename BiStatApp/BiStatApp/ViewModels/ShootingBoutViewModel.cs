using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiStatApp.ViewModels
{
    public class ShootingBoutViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public ShootingBoutViewModel()
        {

        }

        private ShootingBout.PositionEnum _position;
        public ShootingBout.PositionEnum Position
        {
            get { return _position; }
            set
            {
                SetValue(ref _position, value);
                OnPropertyChanged(nameof(Position));
            }
        }

        private bool _alpha;
        public bool Alpha
        { 
            get { return _alpha; }
            set
            {
                SetValue(ref _alpha, value);
                OnPropertyChanged(nameof(Alpha));
            }
        }

        private bool _bravo;
        public bool Bravo
        {
            get { return _bravo; }
            set
            {
                SetValue(ref _bravo, value);
                OnPropertyChanged(nameof(Bravo));
            }
        }

        private bool _charlie;
        public bool Charlie
        {
            get { return _charlie; }
            set
            {
                SetValue(ref _charlie, value);
                OnPropertyChanged(nameof(Charlie));
            }
        }


        private bool _delta;
        public bool Delta
        {
            get { return _delta; }
            set
            {
                SetValue(ref _delta, value);
                OnPropertyChanged(nameof(Delta));
            }
        }

        private bool _echo;
        public bool Echo
        {
            get { return _echo; }
            set
            {
                SetValue(ref _echo, value);
                OnPropertyChanged(nameof(Echo));
            }
        }

    }
}
