using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class ShootingBoutViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public int SessionId { get; set; }

        public ShootingBoutViewModel()
        {

        }

        public ShootingBoutViewModel(ShootingBout bout)
        {
            Id = bout.Id;
            _position = bout.Position;
            _alpha = bout.Alpha;
            _bravo = bout.Bravo;
            _charlie = bout.Charlie;
            _delta = bout.Delta;
            _echo = bout.Echo;
            _startHeartRate = bout.StartHeartRate;
            _endHeartRate = bout.EndHeartRate;
            _duration = Convert.ToDecimal(bout.Duration);
            SessionId = bout.SessionId;
        }

        public ShootingBout CreateShootingBout()
        {
            ShootingBout sb = new ShootingBout()
            {
                Id = Id,
                Position = Position,
                Alpha = Alpha,
                Bravo = Bravo,
                Charlie = Charlie,
                Delta = Delta,
                Echo = Echo,
                StartHeartRate = StartHeartRate,
                EndHeartRate = EndHeartRate,
                Duration = (double)Duration,
                SessionId = SessionId
            };

            return sb;
        }

        private ShootingBout.PositionEnum _position = ShootingBout.PositionEnum.PRONE;
        public ShootingBout.PositionEnum Position
        {
            get => _position;
            set
            {
                SetValue(ref _position, value, "Position");
                OnPropertyChanged("TargetImage");
            }
        }

        private bool _alpha;
        public bool Alpha
        {
            get => _alpha;
            set
            {
                SetValue(ref _alpha, value, "Alpha");
                OnPropertyChanged("TargetImage");
            }
        }

        private bool _bravo;
        public bool Bravo
        {
            get => _bravo;
            set
            {
                SetValue(ref _bravo, value, "Bravo");
                OnPropertyChanged("TargetImage");
            }
        }

        private bool _charlie;
        public bool Charlie
        {
            get => _charlie;
            set
            {
                SetValue(ref _charlie, value, "Charlie");
                OnPropertyChanged("TargetImage");
            }
        }


        private bool _delta;
        public bool Delta
        {
            get => _delta;
            set
            {
                SetValue(ref _delta, value, "Delta");
                OnPropertyChanged("TargetImage");
            }
        }

        private bool _echo;
        public bool Echo
        {
            get => _echo;
            set
            {
                SetValue(ref _echo, value, "Echo");
                OnPropertyChanged("TargetImage");
            }
        }

        private int _startHeartRate;
        public int StartHeartRate
        {
            get => _startHeartRate;
            set => SetValue(ref _startHeartRate, value, "StartHeartRate");
        }

        private int _endHeartRate;
        public int EndHeartRate
        {
            get => _endHeartRate; 
            set => SetValue(ref _endHeartRate, value, "EndHeartRate");
        }

        private decimal _duration;
        public decimal Duration
        { 
            get => _duration; 
            set => SetValue(ref _duration, value, "Duration");
        }


        public int Misses
        {
            get
            {
                int m = Alpha ? 0 : 1;
                m += Bravo ? 0 : 1;
                m += Charlie ? 0 : 1;
                m += Delta ? 0 : 1;
                m += Echo ? 0 : 1;

                return m;
            }
        }

        public ImageSource PositionImage
        {
            get
            {
                if (Position == ShootingBout.PositionEnum.PRONE)
                {
                    return ImageSource.FromResource("BiStatApp.Assets.Images.BiProne.png");
                }
                return ImageSource.FromResource("BiStatApp.Assets.Images.BiStand.png");
            }
        }

        public ImageSource TargetImage
        {
            get
            {
                string imageName = Position == ShootingBout.PositionEnum.PRONE ? "target" : "starget";
                if (Misses < 5)
                {
                    if (Alpha)
                    {
                        imageName += "A";
                    }
                    if (Bravo)
                    {
                        imageName += "B";
                    }
                    if (Charlie)
                    {
                        imageName += "C";
                    }
                    if (Delta)
                    {
                        imageName += "D";
                    }
                    if (Echo)
                    {
                        imageName += "E";
                    }
                    return ImageSource.FromResource("BiStatApp.Assets.Images." + imageName + ".png");
                }
                return ImageSource.FromResource("BiStatApp.Assets.Images." + imageName + "0.png");
            }
        }
    }
}
