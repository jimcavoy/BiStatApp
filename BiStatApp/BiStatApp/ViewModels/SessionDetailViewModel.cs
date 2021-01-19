﻿using BiStatApp.Models;
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
            get { return _selectedShootingBout; }
            set { SetValue(ref _selectedShootingBout, value); }
        }

        public string SessionId
        {
            get
            {
                return _sessionId;
            }
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

        //public ICommand LoadDataCommand { get; private set; }
        //public ICommand SaveCommand { get; private set; }

        //public ICommand AddShootingBoutCommand { get; private set; }

        //public ICommand SelectShootingBoutCommand { get; private set; }

        //public ICommand DeleteShootingBoutCommand { get; private set; }

        public ICommand SendCommand { get; private set; }

        public SessionDetailViewModel()
        {
            SendCommand = new Command(async () => await Send());
        }

        public SessionDetailViewModel(SessionViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            //LoadDataCommand = new Command(async () => await LoadData());
            //SaveCommand = new Command(async () => await Save());
            //AddShootingBoutCommand = new Command(async c => await AddShootingBout());
            //SelectShootingBoutCommand = new Command<ShootingBoutViewModel>(async c => await SelectShootingBout(c));
            //DeleteShootingBoutCommand = new Command<ShootingBoutViewModel>(async c => await DeleteShootingBout(c));
            SendCommand = new Command(async () => await Send());

            //Session = new Session
            //{
            //    Id = viewModel.Id,
            //    Name = viewModel.Name,
            //    Description = viewModel.Description,
            //    DateTime = viewModel.DateTime,
            //    Bouts = viewModel.Bouts.ToList()
            //};

            //MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
            //    (this, Events.ShootingBoutUpdated, OnShootingBoutUpdated);
        }

        //private void OnShootingBoutAdded(ShootingBoutPageViewModel source, ShootingBout bout)
        //{
        //    Unsubscribe();
        //    ShootingBouts.Add(new ShootingBoutViewModel(bout));
        //}

        //private void OnShootingBoutUpdated(ShootingBoutPageViewModel source, ShootingBout bout)
        //{
        //    Unsubscribe();
        //    var boutInList = ShootingBouts.SingleOrDefault(c => c.Id == bout.Id);

        //    if (boutInList != null)
        //    {
        //        boutInList.Position = bout.Position;
        //        boutInList.Alpha = bout.Alpha;
        //        boutInList.Bravo = bout.Bravo;
        //        boutInList.Charlie = bout.Charlie;
        //        boutInList.Delta = bout.Delta;
        //        boutInList.Echo = bout.Echo;
        //        boutInList.StartHeartRate = bout.StartHeartRate;
        //        boutInList.EndHeartRate = bout.EndHeartRate;
        //        boutInList.Duration = Convert.ToDecimal(bout.Duration);
        //    }
        //}

        //private async Task LoadData()
        //{
        //    if (_isDataLoaded)
        //        return;

        //    _isDataLoaded = true;

        //    //await Task.Run(() =>
        //    //{
        //        foreach (var b in Session.Bouts)
        //        {
        //            ShootingBouts.Add(new ShootingBoutViewModel(b));
        //        }
        //    //});
        //}

        //async Task Save()
        //{
        //    if (String.IsNullOrWhiteSpace(Session.Name))
        //    {
        //        await _pageService.DisplayAlert("Error", "Please enter the name.", "OK");
        //        return;
        //    }

        //    if (Session.Id == 0)
        //    {
        //        await DataStore.AddSession(Session);
        //        MessagingCenter.Send(this, Events.SessionAdded, Session);
        //    }
        //    else
        //    {
        //        await DataStore.UpdateSession(Session);
        //        MessagingCenter.Send(this, Events.SessionUpdated, Session);
        //    }
        //    await _pageService.PopAsync();
        //}

        //private async Task AddShootingBout()
        //{
        //    if (Session.Id == 0)
        //    {
        //        await _pageService.DisplayAlert("Warning", $"Save session first before adding a shooting bout.", "Ok");
        //        return;
        //    }
        //    Subscribe();
        //    await _pageService.PushAsync(new ShootingBoutDetailPage(new ShootingBoutViewModel() { SessionId = Session.Id }));
        //}

        //private async Task SelectShootingBout(ShootingBoutViewModel bout)
        //{
        //    if (bout == null)
        //        return;

        //    if (Session.Id == 0)
        //    {
        //        await _pageService.DisplayAlert("Warning", $"Save session first before editing a shooting bout.", "Ok");
        //        return;
        //    }

        //    Subscribe();
        //    SelectedShootingBout = null;
        //    await _pageService.PushAsync(new ShootingBoutDetailPage(bout));
        //}

        //private void Subscribe()
        //{
        //    MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
        //        (this, Events.ShootingBoutAdded, OnShootingBoutAdded);
        //    MessagingCenter.Subscribe<ShootingBoutPageViewModel, ShootingBout>
        //        (this, Events.ShootingBoutUpdated, OnShootingBoutUpdated);
        //}

        //private void Unsubscribe()
        //{
        //    MessagingCenter.Unsubscribe<ShootingBoutPageViewModel, ShootingBout>
        //       (this, Events.ShootingBoutAdded);

        //    MessagingCenter.Unsubscribe<ShootingBoutPageViewModel, ShootingBout>
        //        (this, Events.ShootingBoutUpdated);
        //}

        //private async Task DeleteShootingBout(ShootingBoutViewModel boutViewModel)
        //{
        //    if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete shooting bout?", "Yes", "No"))
        //    {
        //        ShootingBouts.Remove(boutViewModel);

        //        var sb = await _sessionStore.GetShootingBout(boutViewModel.Id);
        //        await _sessionStore.DeleteShootingBout(sb);
        //    }
        //}

        private async Task Send()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            string jsonString = JsonSerializer.Serialize(Session, options);
            Debug.WriteLine(jsonString);

            string fileName = string.Format("{0}_{1:ddMMMyy}.json", Session.Name, Session.DateTime);

            var localPath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllText(localPath, jsonString);

            //await _pageService.PushAsync(new SendSessionPage(_pageService, localPath));
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