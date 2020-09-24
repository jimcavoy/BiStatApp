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
            SessionId = bout.SessionId;
        }

        private ShootingBout.PositionEnum _position = ShootingBout.PositionEnum.PRONE;
        public ShootingBout.PositionEnum Position
        {
            get { return _position; }
            set
            {
                SetValue(ref _position, value);
                OnPropertyChanged(nameof(TargetImage));
            }
        }

        private bool _alpha;
        public bool Alpha
        { 
            get { return _alpha; }
            set
            {
                SetValue(ref _alpha, value);
                OnPropertyChanged(nameof(TargetImage));
            }
        }

        private bool _bravo;
        public bool Bravo
        {
            get { return _bravo; }
            set
            {
                SetValue(ref _bravo, value);
                OnPropertyChanged(nameof(TargetImage));
            }
        }

        private bool _charlie;
        public bool Charlie
        {
            get { return _charlie; }
            set
            {
                SetValue(ref _charlie, value);
                OnPropertyChanged(nameof(TargetImage));
            }
        }


        private bool _delta;
        public bool Delta
        {
            get { return _delta; }
            set
            {
                SetValue(ref _delta, value);
                OnPropertyChanged(nameof(TargetImage));
            }
        }

        private bool _echo;
        public bool Echo
        {
            get { return _echo; }
            set
            {
                SetValue(ref _echo, value);
                OnPropertyChanged(nameof(TargetImage));
            }
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
                if (Position == ShootingBout.PositionEnum.PRONE)
                {
                    return GetProneTarget();
                }
                return GetStandingTarget();
            }
        }

        private ImageSource GetProneTarget()
        {
            if (Alpha && !Bravo && !Charlie && !Delta && !Echo)
            {
                return ImageSource.FromResource("BiStatApp.Assets.Images.targetA.png");
            }
            return ImageSource.FromResource("BiStatApp.Assets.Images.target0.png");
        }

        private ImageSource GetStandingTarget()
        {
            if (Alpha && !Bravo && !Charlie && !Delta && !Echo)
            {
                return ImageSource.FromResource("BiStatApp.Assets.Images.stargetA.png");
            }
            return ImageSource.FromResource("BiStatApp.Assets.Images.starget0.png");
        }
    }
}
