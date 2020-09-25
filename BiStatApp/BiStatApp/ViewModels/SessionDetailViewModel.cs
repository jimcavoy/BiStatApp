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

        public ICommand DeleteShootingBoutCommand { get; private set; }

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
            DeleteShootingBoutCommand = new Command<ShootingBoutViewModel>(async c => await DeleteShootingBout(c));

            Session = new Session
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                DateTime = viewModel.DateTime,
                Bouts = viewModel.Bouts.ToList()
            };

            MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
                (this, Events.ShootingBoutUpdated, OnShootingBoutUpdated);
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
            }
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
            if (Session.Id == 0)
            {
                await _pageService.DisplayAlert("Warning", $"Save session first before adding a shooting bout.", "Ok");
                return;
            }
            Subscribe();
            await _pageService.PushAsync(new ShootingBoutDetailPage(new ShootingBoutViewModel() { SessionId = Session.Id }));
        }

        private async Task SelectShootingBout(ShootingBoutViewModel bout)
        {
            if (bout == null)
                return;

            if (Session.Id == 0)
            {
                await _pageService.DisplayAlert("Warning", $"Save session first before editing a shooting bout.", "Ok");
                return;
            }

            Subscribe();
            SelectedShootingBout = null;
            await _pageService.PushAsync(new ShootingBoutDetailPage(bout));
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

        private async Task DeleteShootingBout(ShootingBoutViewModel boutViewModel)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete shooting bout?", "Yes", "No"))
            {
                ShootingBouts.Remove(boutViewModel);

                var sb = await _sessionStore.GetShootingBout(boutViewModel.Id);
                await _sessionStore.DeleteShootingBout(sb);
            }
        }
    }

}