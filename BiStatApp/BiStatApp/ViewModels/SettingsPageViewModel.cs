using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        private readonly ISessionStore _sessionStore;

        public ICommand SendReportCommand { get; private set; }

        public ICommand ExportDataCommand { get; private set; }

        public ICommand ImportDataCommand { get; private set; }

        public ImageSource BannerImage
        { 
            get
            {
                return ImageSource.FromResource("BiStatApp.Assets.Images.xamarin_logo.png");
            }
        }

        public string CurrentVersion { get; private set; }

        public SettingsPageViewModel(ISessionStore sessionStore, IPageService pageService)
        {
            _sessionStore = sessionStore;
            _pageService = pageService;

            SendReportCommand = new Command(async () => await SendReport());
            ExportDataCommand = new Command(async () => await ExportData());
            ImportDataCommand = new Command(async () => await ImportData());

            CurrentVersion = VersionTracking.CurrentVersion;
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

        private async Task ExportData()
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.WriteIndented = true;
                BiStatDocument doc = await _sessionStore.GetDocument();
                doc.Version = CurrentVersion;
                string jsonString = JsonSerializer.Serialize(doc, options);

                string fileName = "BiStatDocument.json";

                var localPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                File.WriteAllText(localPath, jsonString);
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ImportData()
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                string fileName = "BiStatDocument.json";

                var localPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                string jsonString = File.ReadAllText(localPath);
                var doc = JsonSerializer.Deserialize<BiStatDocument>(jsonString, options);

                await _sessionStore.SetDocument(doc);
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
