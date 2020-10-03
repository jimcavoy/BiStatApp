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

            if (practice.Name != "Dry Fire")
            {
                session.Bouts.Add(new ShootingBout()
                {
                    Position = ShootingBout.PositionEnum.PRONE,
                    Alpha = false,
                    Bravo = false,
                    Charlie = false,
                    Delta = false,
                    Echo = false,
                });
            }

            session = await _sessionStore.AddSession(session);

            await _pageService.PushAsync(new SessionDetailPage(new SessionViewModel(session)));
        }

    }
}
