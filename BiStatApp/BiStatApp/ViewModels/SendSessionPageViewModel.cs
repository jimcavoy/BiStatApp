using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    [QueryProperty(nameof(Filepath), nameof(Filepath))]
    public class SendSessionPageViewModel : BaseViewModel
    {
        private string _filepath;
        public string Filepath
        {
            get => _filepath;
            set { _filepath = value; }
        }

        public string Subject { get; set; } = "";

        public string Recipients { get; set; } = "";

        public string Body { get; set; } = "";

        public Session Session { get; private set; }

        public ICommand SendEmailCommand { get; private set; }

        public SendSessionPageViewModel()
        {
            SendEmailCommand = new Command(async () => await Send());
        }

        private async Task Send()
        {
            try
            {
                List<string> toAddress = new List<string>
                {
                    Recipients
                };

                var message = new EmailMessage
                {
                    Subject = Subject,
                    Body = Body,
                    To = toAddress
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
