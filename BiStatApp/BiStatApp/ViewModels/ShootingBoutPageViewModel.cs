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

        public ShootingBout Bout { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ShootingBoutPageViewModel(ShootingBoutViewModel viewModel, ISessionStore sessionStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _sessionStore = sessionStore;

            SaveCommand = new Command(async () => await Save());

            Bout = new ShootingBout
            {
                Id = viewModel.Id,
                SessionId = viewModel.SessionId,
                Alpha = viewModel.Alpha,
                Bravo = viewModel.Bravo,
                Charlie = viewModel.Charlie,
                Delta = viewModel.Delta,
                Echo = viewModel.Echo,
                Position = viewModel.Position
            };
        }

        async Task Save()
        {
            if (Bout.Id == 0)
            {
                await _sessionStore.AddShootingBout(Bout);
                MessagingCenter.Send(this, Events.ShootingBoutAdded, Bout);
            }
            else
            {
                await _sessionStore.UpdateShootingBout(Bout);
                MessagingCenter.Send(this, Events.ShootingBoutUpdated, Bout);
            }
            await _pageService.PopAsync();
        }
    }
}
