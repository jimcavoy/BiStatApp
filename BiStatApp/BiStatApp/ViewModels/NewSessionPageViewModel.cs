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
    public class NewSessionPageViewModel : BaseViewModel
    {
        private ShootingBoutViewModel _selectedShootingBout;
        private string _sessionId;
        private SessionViewModel _session;

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

        public SessionViewModel Session 
        { 
            get => _session; 
            set => SetValue(ref _session, value, "Session"); 
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
                Name = Session.Name,
                Description = Session.Description,
                DateTime = Session.DateTime
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
            ShootingBout newSb = new ShootingBout()
            {
                SessionId = int.Parse(SessionId)
            };
            newSb = await DataStore.AddShootingBout(newSb);
            ShootingBouts.Add(new ShootingBoutViewModel(newSb));
            await Shell.Current.GoToAsync($"{nameof(ShootingBoutDetailPage)}?{nameof(ShootingBoutPageViewModel.BoutId)}={newSb.Id}");
        }

        private async void OnSelectShootingBout(ShootingBoutViewModel bout)
        {
            if (bout == null)
                return;

            SelectedShootingBout = null;
            await Shell.Current.GoToAsync($"{nameof(ShootingBoutDetailPage)}?{nameof(ShootingBoutPageViewModel.BoutId)}={bout.Id}");
        }

        private async Task DeleteShootingBout(ShootingBoutViewModel bout)
        {
            if (await Application.Current.MainPage.DisplayAlert("Warning", $"Are you sure you want to delete shooting bout?", "Yes", "No"))
            {
                ShootingBouts.Remove(bout);

                var sb = await DataStore.GetShootingBout(bout.Id);
                await DataStore.DeleteShootingBout(sb);
            }
        }

        public async void LoadData(string sessionId)
        {
            if (sessionId == null)
                return;

            int id = int.Parse(sessionId);
            var s = await DataStore.GetSession(id);
            ShootingBouts.Clear();

            if (s != null)
            {
                foreach (var b in s.Bouts)
                {
                    ShootingBouts.Add(new ShootingBoutViewModel(b));
                }
            }

            if (Session == null)
                Session = new SessionViewModel(s);
        }
    }
}
