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
    [QueryProperty(nameof(Filepath), nameof(Filepath))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendSessionPage : ContentPage
    {
        private string _filepath;

        public string Filepath
        {
            get => _filepath;
            set => _filepath = value;
        }

        public SendSessionPage()
        {
            InitializeComponent();
        }

        private async void OnSendBtnClicked(object sender, EventArgs e)
        {
            List<string> toAddress = new List<string>
            {
                textTo.Text
            };
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
                await Shell.Current.GoToAsync("..");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", ex.Message, "Ok");
            }
        }
    }
}