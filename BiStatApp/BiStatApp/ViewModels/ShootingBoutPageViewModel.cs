using BiStatApp.Models;
using BiStatApp.Views;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    [QueryProperty(nameof(BoutId), nameof(BoutId))]
    public class ShootingBoutPageViewModel : BaseViewModel
    {
        private string _boutId;
        public ShootingBoutViewModel Bout { get; private set; }

        bool _showAdvanceView = false;
        public bool ShowAdvanceView
        {
            get => _showAdvanceView;
            set => SetValue(ref _showAdvanceView, value);
        }

        public string BoutId
        {
            get => _boutId;
            set
            {
                _boutId = value;
                LoadData(value);
            }
        }


        public ICommand SaveCommand { get; private set; }

        public ICommand AlphaCheckedCommand { get; private set; }

        public ICommand BravoCheckedCommand { get; private set; }

        public ICommand CharlieCheckedCommand { get; private set; }

        public ICommand DeltaCheckedCommand { get; private set; }

        public ICommand EchoCheckedCommand { get; private set; }

        public ICommand PositionChangedCommand { get; private set; }

        public ShootingBoutPageViewModel()
        {
            SaveCommand = new Command(async () => await Save());
            AlphaCheckedCommand = new Command<bool>(async c => await AlphaChecked(c));
            BravoCheckedCommand = new Command<bool>(async c => await BravoChecked(c));
            CharlieCheckedCommand = new Command<bool>(async c => await CharlieChecked(c));
            DeltaCheckedCommand = new Command<bool>(async c => await DeltaChecked(c));
            EchoCheckedCommand = new Command<bool>(async c => await EchoChecked(c));
            PositionChangedCommand = new Command<bool>(async c => await PositionChanged(c));
        }

        async Task Save()
        {
            ShootingBout aBout = new ShootingBout
            {
                Id = Bout.Id,
                SessionId = Bout.SessionId,
                Alpha = Bout.Alpha,
                Bravo = Bout.Bravo,
                Charlie = Bout.Charlie,
                Delta = Bout.Delta,
                Echo = Bout.Echo,
                Position = Bout.Position,
                StartHeartRate = Bout.StartHeartRate,
                EndHeartRate = Bout.EndHeartRate,
                Duration = Convert.ToDouble(Bout.Duration)
            };

            if (Bout.Id == 0)
            {
                await DataStore.AddShootingBout(aBout);
                MessagingCenter.Send(this, Events.ShootingBoutAdded, aBout);
            }
            else
            {
                await DataStore.UpdateShootingBout(aBout);
                MessagingCenter.Send(this, Events.ShootingBoutUpdated, aBout);
            }
            await Shell.Current.GoToAsync("..");
        }

        async Task AlphaChecked(bool value)
        {
            await Task.Run(() => { Bout.Alpha = value; });
        }

        async Task BravoChecked(bool value)
        {
            await Task.Run(() => { Bout.Bravo = value; });
        }

        async Task CharlieChecked(bool value)
        {
            await Task.Run(() => { Bout.Charlie = value; });
        }

        async Task DeltaChecked(bool value)
        {
            await Task.Run(() => { Bout.Delta = value; });
        }

        async Task EchoChecked(bool value)
        {
            await Task.Run(() => { Bout.Echo = value; });
        }

        async Task PositionChanged(bool value)
        {
            await Task.Run(() => { Bout.Position = value ? ShootingBout.PositionEnum.STANDING : ShootingBout.PositionEnum.PRONE; });
        }

        private async void LoadData(string boutId)
        {
            int id = int.Parse(boutId);
            var b = await DataStore.GetShootingBout(id);

            if (b != null)
            {
                Bout = new ShootingBoutViewModel
                {
                    Id = b.Id,
                    SessionId = b.SessionId,
                    Alpha = b.Alpha,
                    Bravo = b.Bravo,
                    Charlie = b.Charlie,
                    Delta = b.Delta,
                    Echo = b.Echo,
                    Position = b.Position,
                    StartHeartRate = b.StartHeartRate,
                    EndHeartRate = b.EndHeartRate,
                    Duration = (decimal)b.Duration
                };

                Title = "Edit";
            }
            else
            {
                Bout = new ShootingBoutViewModel();
                Title = "New";
            }
        }
    }
}
