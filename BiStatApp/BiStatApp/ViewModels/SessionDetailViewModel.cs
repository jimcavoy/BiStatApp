using BiStatApp.Models;
using BiStatApp.Views;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class SessionDetailViewModel : BaseViewModel
    {
        private ShootingBoutViewModel _selectedShootingBout;
        private readonly ISessionStore _sessionStore;
        private readonly IPageService _pageService;

        public Session Session { get; private set; }

        public ShootingBoutViewModel SelectedShootingBout
        {
            get { return _selectedShootingBout; }
            set { SetValue(ref _selectedShootingBout, value); }
        }

        private bool _isDataLoaded = false;

        public ObservableCollection<ShootingBoutViewModel> ShootingBouts { get; private set; }
            = new ObservableCollection<ShootingBoutViewModel>();

        public ICommand LoadDataCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public ICommand AddShootingBoutCommand { get; private set; }

        public ICommand SelectShootingBoutCommand { get; private set; }

        public SessionDetailViewModel(SessionViewModel viewModel, ISessionStore sessionStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _sessionStore = sessionStore;

            LoadDataCommand = new Command(async () => await LoadData());
            SaveCommand = new Command(async () => await Save());
            AddShootingBoutCommand = new Command(async c => await AddShootingBout());
            SelectShootingBoutCommand = new Command<ShootingBoutViewModel>(async c => await SelectShootingBout(c));

            Session = new Session
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                DateTime = viewModel.DateTime,
                Bouts = viewModel.Bouts.ToList()
            };

            MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
                (this, Events.ShootingBoutAdded, OnShootingBoutAdded);

            MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
                (this, Events.ShootingBoutUpdated, OnShootingBoutUpdated);
        }

        private void OnShootingBoutAdded(ShootingBoutPageViewModel source, ShootingBout bout)
        {
            bout.SessionId = Session.Id;
            _sessionStore.AddShootingBout(bout);
        }

        private void OnShootingBoutUpdated(ShootingBoutPageViewModel source, ShootingBout bout)
        {
            var boutInList = ShootingBouts.Single(c => c.Id == bout.Id);

            boutInList.Position = bout.Position;
            boutInList.Alpha = bout.Alpha;
            boutInList.Bravo = bout.Bravo;
            boutInList.Charlie = bout.Charlie;
            boutInList.Delta = bout.Delta;
            boutInList.Echo = bout.Echo;
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;
            foreach (var b in Session.Bouts)
            {
                ShootingBouts.Add(new ShootingBoutViewModel(b));
            }
        }

        async Task Save()
        {
            if (String.IsNullOrWhiteSpace(Session.Name))
            {
                await _pageService.DisplayAlert("Error", "Please enter the name.", "OK");
                return;
            }

            if (Session.Id == 0)
            {
                await _sessionStore.AddSession(Session);
                MessagingCenter.Send(this, Events.SessionAdded, Session);
            }
            else
            {
                await _sessionStore.UpdateSession(Session);
                MessagingCenter.Send(this, Events.SessionUpdated, Session);
            }
            await _pageService.PopAsync();
        }

        private async Task AddShootingBout()
        {
            await _pageService.PushAsync(new ShootingBoutDetailPage());
        }

        private async Task SelectShootingBout(ShootingBoutViewModel bout)
        {
            if (bout == null)
                return;

            SelectedShootingBout = null;
            await _pageService.PushAsync(new ShootingBoutDetailPage());
        }
    }

}