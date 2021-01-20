using BiStatApp.Models;
using BiStatApp.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class PracticesPageViewModel : BaseViewModel
    {
        private Practice _selectedPractice;

        private bool _isDataLoaded = false;

        public ObservableCollection<Practice> Practices { get; private set; }
            = new ObservableCollection<Practice>();

        public Practice SelectedPractice
        {
            get =>_selectedPractice; 
            set 
            { 
                SetValue(ref _selectedPractice, value);
                OnSelectPractice(value);
            }
        }

        public ICommand LoadDataCommand { get; private set; }

        public PracticesPageViewModel()
        {
            LoadDataCommand = new Command(async () => await LoadData());
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;
            var practices = await DataStore.GetPracticesAsync();
            foreach (var p in practices)
            {
                Practices.Add(p);
            }
        }

        private async void OnSelectPractice(Practice practice)
        {
            if (practice == null)
            {
                return;
            }
            SelectedPractice = null;

            var session = new Session()
            {
                Name = practice.Name,
                DateTime = System.DateTime.Now
            };

            AddShootingBouts(session, practice.Name);

            session = await DataStore.AddSession(session);
            await Shell.Current.GoToAsync($"{nameof(NewSessionPage)}?{nameof(NewSessionPageViewModel.SessionId)}={session.Id}");
        }

        private void AddShootingBouts(Session session, string practiceType)
        {
            if (practiceType == "Dry Fire")
                return;

            session.Bouts.Add(new ShootingBout());

            if (practiceType == "Race - Sprint")
            {
                session.Bouts.Add(new ShootingBout() { Position = ShootingBout.PositionEnum.STANDING });
            }
            else if (practiceType == "Race - Pursuit" ||
                practiceType == "Race - Mass Start" ||
                practiceType == "Race - Relay" ||
                practiceType == "Time Trail" ||
                practiceType == "Combo")
            {
                session.Bouts.Add(new ShootingBout());
                session.Bouts.Add(new ShootingBout() { Position = ShootingBout.PositionEnum.STANDING });
                session.Bouts.Add(new ShootingBout() { Position = ShootingBout.PositionEnum.STANDING });
            }
            else if (practiceType == "Race - Individual")
            {
                session.Bouts.Add(new ShootingBout() { Position = ShootingBout.PositionEnum.STANDING });
                session.Bouts.Add(new ShootingBout());
                session.Bouts.Add(new ShootingBout() { Position = ShootingBout.PositionEnum.STANDING });
            }
            else
            {
                session.Bouts.Add(new ShootingBout() { Position = ShootingBout.PositionEnum.STANDING });
            }
        }
    }
}
