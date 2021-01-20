using BiStatApp.Models;
using BiStatApp.Views;

using System;
using System.Diagnostics;
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

        public ShootingBoutPageViewModel()
        {
            SaveCommand = new Command(async () => await Save());
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

            MessagingCenter.Send(this, Events.ShootingBoutUpdated);
        }
    }
}
