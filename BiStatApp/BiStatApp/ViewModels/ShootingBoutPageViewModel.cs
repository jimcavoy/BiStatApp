using BiStatApp.Models;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class ShootingBoutPageViewModel
    {
        private readonly ISessionStore _sessionStore;
        private readonly IPageService _pageService;

        public ShootingBoutViewModel Bout { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand AlphaCheckedCommand { get; private set; }

        public ICommand BravoCheckedCommand { get; private set; }

        public ICommand CharlieCheckedCommand { get; private set; }

        public ICommand DeltaCheckedCommand { get; private set; }

        public ICommand EchoCheckedCommand { get; private set; }

        public ICommand PositionChangedCommand { get; private set; }

        public ShootingBoutPageViewModel(ShootingBoutViewModel viewModel, ISessionStore sessionStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _sessionStore = sessionStore;

            SaveCommand = new Command(async () => await Save());
            AlphaCheckedCommand = new Command<bool>(async c => await AlphaChecked(c));
            BravoCheckedCommand = new Command<bool>(async c => await BravoChecked(c));
            CharlieCheckedCommand = new Command<bool>(async c => await CharlieChecked(c));
            DeltaCheckedCommand = new Command<bool>(async c => await DeltaChecked(c));
            EchoCheckedCommand = new Command<bool>(async c => await EchoChecked(c));
            PositionChangedCommand = new Command<bool>(async c => await PositionChanged(c));

            Bout = new ShootingBoutViewModel
            {
                Id = viewModel.Id,
                SessionId = viewModel.SessionId,
                Alpha = viewModel.Alpha,
                Bravo = viewModel.Bravo,
                Charlie = viewModel.Charlie,
                Delta = viewModel.Delta,
                Echo = viewModel.Echo,
                Position = viewModel.Position,
                StartHeartRate = viewModel.StartHeartRate,
                EndHeartRate = viewModel.EndHeartRate,
                Duration = viewModel.Duration
            };
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
                await _sessionStore.AddShootingBout(aBout);
                MessagingCenter.Send(this, Events.ShootingBoutAdded, aBout);
            }
            else
            {
                await _sessionStore.UpdateShootingBout(aBout);
                MessagingCenter.Send(this, Events.ShootingBoutUpdated, aBout);
            }
            await _pageService.PopAsync();
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
    }
}
