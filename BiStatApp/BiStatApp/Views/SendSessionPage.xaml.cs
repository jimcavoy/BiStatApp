using BiStatApp.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BiStatApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendSessionPage : ContentPage
    {
        private string _filepath;
        private readonly IPageService _pageService;

        public SendSessionPage(IPageService pageService, string filepath)
        {
            InitializeComponent();
            _pageService = pageService;
            _filepath = filepath;
        }

        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            List<string> toAddress = new List<string>();
            toAddress.Add(textTo.Text);
            await SendEmail(textSubject.Text, textBody.Text, toAddress);
        }

        private async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients
                };
                message.Attachments.Add(new EmailAttachment(_filepath));
                await Email.ComposeAsync(message);
                await _pageService.PopAsync();

            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Warning", ex.Message, "Ok");
            }
        }
    }
}