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

        public ShootingBoutPageViewModel(ShootingBout viewModel, ISessionStore sessionStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _sessionStore = sessionStore;

            SaveCommand = new Command(async () => await Save());
        }

        async Task Save()
        {
            if (Bout.Id == 0)
            {

            }
            else
            {

            }
            await _pageService.PopAsync();
        }
    }
}
