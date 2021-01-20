using BiStatApp.Models;
using BiStatApp.Views;
using BiStatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace BiStatApp.ViewModels
{
    [QueryProperty(nameof(SessionId), nameof(SessionId))]
    class NewSessionPageViewModel : BaseViewModel
    {
        private ShootingBoutViewModel _selectedShootingBout;
        private string _sessionId;
        private string _sessionName;
        private string _description;
        private DateTime _dateTime;

        public string SessionId
        {
            get => _sessionId;
            set
            {
                if (value != null)
                {
                    _sessionId = value;
                    LoadData(value); 
                }
            }
        }

        public string SessionName
        {
            get => _sessionName;
            set => SetValue(ref _sessionName, value);
        }

        public string Description
        {
            get => _description;
            set => SetValue(ref _description, value);
        }

        public DateTime DateTime
        {
            get => _dateTime;
            set => SetValue(ref _dateTime, value);
        }

        public ShootingBoutViewModel SelectedShootingBout
        {
            get => _selectedShootingBout;
            set
            {
                SetValue(ref _selectedShootingBout, value);
                OnSelectShootingBout(value);
            }
        }

        public ObservableCollection<ShootingBoutViewModel> ShootingBouts { get; private set; }
            = new ObservableCollection<ShootingBoutViewModel>();

        public ICommand SaveCommand { get; private set; }

        public ICommand AddShootingBoutCommand { get; private set; }

        public ICommand DeleteShootingBoutCommand { get; private set; }

        public NewSessionPageViewModel()
        {
            SaveCommand = new Command(async () => await Save());
            AddShootingBoutCommand = new Command(async c => await AddShootingBout());
            DeleteShootingBoutCommand = new Command<ShootingBoutViewModel>(async c => await DeleteShootingBout(c));
        }

        async Task Save()
        {
            Session us = new Session()
            {
                Id = int.Parse(SessionId),
                Name = SessionName,
                Description = Description,
                DateTime = DateTime
            };

            foreach (var sb in ShootingBouts)
            {
                us.Bouts.Add(sb.CreateShootingBout());
            }

            await DataStore.UpdateSession(us);
            await Shell.Current.GoToAsync("..");
        }

        private async Task AddShootingBout()
        {
            Subscribe();
            ShootingBout newSb = new ShootingBout()
            {
                SessionId = int.Parse(SessionId)
            };
            newSb = await DataStore.AddShootingBout(newSb);
            ShootingBouts.Add(new ShootingBoutViewModel(newSb));
            await Shell.Current.GoToAsync($"{nameof(ShootingBoutDetailPage)}?{nameof(ShootingBoutPageViewModel.BoutId)}={newSb.Id.ToString()}");
        }

        private async void OnSelectShootingBout(ShootingBoutViewModel bout)
        {
            if (bout == null)
                return;

            Subscribe();
            SelectedShootingBout = null;
            await Shell.Current.GoToAsync($"{nameof(ShootingBoutDetailPage)}?{nameof(ShootingBoutPageViewModel.BoutId)}={bout.Id.ToString()}");
        }

        private async Task DeleteShootingBout(ShootingBoutViewModel bout)
        {

        }

        private void OnShootingBoutAdded(ShootingBoutPageViewModel source, ShootingBout bout)
        {
            Unsubscribe();
            ShootingBouts.Add(new ShootingBoutViewModel(bout));
        }

        private void OnShootingBoutUpdated(ShootingBoutPageViewModel source, ShootingBout bout)
        {
            Unsubscribe();
            var boutInList = ShootingBouts.SingleOrDefault(c => c.Id == bout.Id);

            if (boutInList != null)
            {
                boutInList.Position = bout.Position;
                boutInList.Alpha = bout.Alpha;
                boutInList.Bravo = bout.Bravo;
                boutInList.Charlie = bout.Charlie;
                boutInList.Delta = bout.Delta;
                boutInList.Echo = bout.Echo;
                boutInList.StartHeartRate = bout.StartHeartRate;
                boutInList.EndHeartRate = bout.EndHeartRate;
                boutInList.Duration = Convert.ToDecimal(bout.Duration);
            }
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
                (this, Events.ShootingBoutAdded, OnShootingBoutAdded);
            MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
                (this, Events.ShootingBoutUpdated, OnShootingBoutUpdated);
        }

        private void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<ShootingBoutPageViewModel, ShootingBout>
               (this, Events.ShootingBoutAdded);
            MessagingCenter.Unsubscribe<ShootingBoutPageViewModel, ShootingBout>
                (this, Events.ShootingBoutUpdated);
        }

        public async void LoadData(string sessionId)
        {
            if (sessionId == null)
                return;

            int id = int.Parse(sessionId);
            var Session = await DataStore.GetSession(id);

            if (Session != null)
            {
                SessionName = Session.Name;
                Description = Session.Description;
                DateTime = Session.DateTime;
                foreach (var b in Session.Bouts)
                {
                    ShootingBouts.Add(new ShootingBoutViewModel(b));
                }
            }
        }
    }
}
