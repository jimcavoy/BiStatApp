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
        private ShootingBoutViewModel _selectedShootingBout;
        private string _sessionId;
        private string _sessionName;
        private string _description;
        private string _dateTime;
        public Session Session { get; private set; }
        public ShootingBoutViewModel SelectedShootingBout
        {
            get => _selectedShootingBout;
            set => SetValue(ref _selectedShootingBout, value);
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

        public string SessionName
        {
            get => _sessionName;
            set => SetValue(ref _sessionName, value);
        }

        public string Description
        {
            get => _description;
            set => SetValue(ref _description, value);
        }

        public string DateTime
        {
            get => _dateTime;
            set => SetValue(ref _dateTime, value);
        }


        public ObservableCollection<ShootingBoutViewModel> ShootingBouts { get; private set; }
            = new ObservableCollection<ShootingBoutViewModel>();

        public ICommand SendCommand { get; private set; }

        public SessionDetailViewModel()
        {
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
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            string jsonString = JsonSerializer.Serialize(Session, options);
            Debug.WriteLine(jsonString);

            string fileName = string.Format("{0}_{1}.json", SessionName, DateTime);

            var localPath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(localPath, jsonString);

            await Shell.Current.GoToAsync("..");
        }

        public async void LoadData(string sessionId)
        {
            int id = int.Parse(sessionId);
            var Session = await DataStore.GetSession(id);

            if (Session != null)
            {
                SessionName = Session.Name;
                Description = Session.Description;
                DateTime = Session.DateTime.ToString("dd MMMM yyy");
                foreach (var b in Session.Bouts)
                {
                    ShootingBouts.Add(new ShootingBoutViewModel(b));
                }
            }
        }
    }

}