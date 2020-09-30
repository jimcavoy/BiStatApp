using BiStatApp.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class MainPageMasterViewModel : BaseViewModel
    {
        public ObservableCollection<MainPageMasterMenuItem> MenuItems { get; set; }

        public MainPageMasterViewModel()
        {
            MenuItems = new ObservableCollection<MainPageMasterMenuItem>(new[]
            {
                new MainPageMasterMenuItem { Id = 0, Title = "Train", TargetType = typeof(PracticesPage), Icon = ImageSource.FromResource("BiStatApp.Assets.Images.target.png")},
                new MainPageMasterMenuItem { Id = 1, Title = "History", TargetType = typeof(SessionsPage), Icon = ImageSource.FromResource("BiStatApp.Assets.Images.todo.png")},
                new MainPageMasterMenuItem { Id = 2, Title = "Settings", TargetType = typeof(SettingsPage), Icon = ImageSource.FromResource("BiStatApp.Assets.Images.settings.png")}
            });
        }
    }
}
