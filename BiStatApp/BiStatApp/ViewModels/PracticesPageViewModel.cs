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
        private readonly ISessionStore _sessionStore;
        private readonly IPageService _pageService;

        private bool _isDataLoaded = false;

        public ObservableCollection<Practice> Practices { get; private set; }
            = new ObservableCollection<Practice>();

        public Practice SelectedPractice
        {
            get { return _selectedPractice; }
            set { SetValue(ref _selectedPractice, value); }
        }

        public ICommand LoadDataCommand { get; private set; }

        public ICommand SelectPracticeCommand { get; private set; }

        public PracticesPageViewModel(ISessionStore sessionStore, IPageService pageService)
        {
            _sessionStore = sessionStore;
            _pageService = pageService;

            LoadDataCommand = new Command(async () => await LoadData());
            SelectPracticeCommand = new Command<Practice>(async c => await SelectPractice(c));
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;
            var practices = await _sessionStore.GetPracticesAsync();
            foreach (var p in practices)
            {
                Practices.Add(p);
            }
        }

        private async Task SelectPractice(Practice practice)
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

            session = await _sessionStore.AddSession(session);

            await _pageService.PushAsync(new SessionDetailPage(new SessionViewModel(session)));
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
