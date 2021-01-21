using BiStatApp.Models;
using BiStatApp.Views;
using Plugin.Toast;
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

        public SettingsPageViewModel()
        {
            Title = "Settings";

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
                await Application.Current.MainPage.DisplayAlert("Warning", ex.Message, "Ok");
            }
        }

        private async Task ExportData()
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.WriteIndented = true;
                BiStatDocument doc = await DataStore.GetDocument();
                doc.Version = CurrentVersion;
                string jsonString = JsonSerializer.Serialize(doc, options);

                string fileName = "BiStatDocument.json";

                var localPath = Path.Combine(FileSystem.CacheDirectory, fileName);
                File.WriteAllText(localPath, jsonString);

                await Shell.Current.GoToAsync($"{nameof(SendSessionPage)}?{nameof(SendSessionPageViewModel.Filepath)}={localPath}");

                //CrossToastPopUp.Current.ShowToastMessage("Sessions Backup");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ImportData()
        {
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    IFileHelper reader = DependencyService.Get<IFileHelper>();
                    if (await reader.CopyFileAsync("", result.FullPath))
                    {
                        string jsonString = await reader.ReadTextAsync(result.FileName);

                        var options = new JsonSerializerOptions();
                        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                        var doc = JsonSerializer.Deserialize<BiStatDocument>(jsonString, options);

                        await DataStore.SetDocument(doc); 
                        CrossToastPopUp.Current.ShowToastMessage("Sessions Restored");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
