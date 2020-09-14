using BiStatApp.Models;
using BiStatApp.ViewModels;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
	public class SessionDetailViewModel
	{
        private readonly ISessionStore _sessionStore;
        private readonly IPageService _pageService;

        public Session Session { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public SessionDetailViewModel(SessionViewModel viewModel, ISessionStore sessionStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _sessionStore = sessionStore;

            SaveCommand = new Command(async () => await Save());

            Session = new Session
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                DateTime = viewModel.DateTime,
                Bouts = viewModel.Bouts.ToList()
            };
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
    }

}