using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private readonly IPageService _pageService;
        public ICommand SendReportCommand { get; private set; }

        public ImageSource BannerImage
        { 
            get
            {
                return ImageSource.FromResource("BiStatApp.Assets.Images.xamarin_logo.png");
            }
        }

        public SettingsPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
            SendReportCommand = new Command(async () => await SendReport());
        }

        private async Task SendReport()
        {
            try
            {
                List<string> recipients = new List<string>()
                {
                    "jimcavoy@thetastream.com"
                };

                var message = new EmailMessage
                {
                    Subject = "Biathlon Shooting Stats Report",
                    To = recipients
                };
                await Email.ComposeAsync(message);

            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Warning", ex.Message, "Ok");
            }
        }
    }
}
