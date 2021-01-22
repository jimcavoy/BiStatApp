using BiStatApp.Models;
using BiStatApp.Views;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    [QueryProperty(nameof(SessionId), nameof(SessionId))]
    public class SessionDetailViewModel : BaseViewModel
    {
        private string _sessionId;
        private SessionViewModel _session;

        public SessionViewModel Session 
        { 
            get => _session; 
            set => SetValue(ref _session, value, "Session");
        }

        public string SessionId
        {
            get => _sessionId;
            set
            {
                _sessionId = value;
                LoadData(value);
            }
        }

        public ObservableCollection<ShootingBoutViewModel> ShootingBouts { get; private set; }
            = new ObservableCollection<ShootingBoutViewModel>();

        public ICommand SaveCommand { get; private set; }
        public ICommand SendCommand { get; private set; }

        public SessionDetailViewModel()
        {
            SaveCommand = new Command(async () => await Save());
            SendCommand = new Command(async () => await Send());
        }

        public SessionDetailViewModel(SessionViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            SendCommand = new Command(async () => await Send());
        }

        private async Task Send()
        {
            Session s = await DataStore.GetSession(int.Parse(SessionId));
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            string jsonString = JsonSerializer.Serialize(s, options);
            Debug.WriteLine(jsonString);

            string fileName = string.Format("{0}_{1:ddMMMyy}.json", Session.Name, Session.DateTime);

            var localPath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(localPath, jsonString);

            await Shell.Current.GoToAsync($"{nameof(SendSessionPage)}?{nameof(SendSessionPageViewModel.Filepath)}={localPath}");
        }

        private async Task Save()
        {
            Session us = new Session()
            {
                Id = int.Parse(SessionId),
                Name = Session.Name,
                Description = Session.Description,
                DateTime = Session.DateTime
            };
            await DataStore.UpdateSession(us);
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadData(string sessionId)
        {
            if (sessionId == null)
                return;

            int id = int.Parse(sessionId);
            var s = await DataStore.GetSession(id);
            if (s != null)
            {
                Session = new SessionViewModel(s);
                foreach (var b in s.Bouts)
                {
                    ShootingBouts.Add(new ShootingBoutViewModel(b));
                }
            }
        }
    }

}