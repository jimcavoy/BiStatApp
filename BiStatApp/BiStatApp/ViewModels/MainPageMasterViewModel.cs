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
            Color color = Color.DarkGray;
            var ret = Application.Current.Resources.TryGetValue("MediumGrayTextColor", out var outValue);
            if (ret && outValue is Color)
            {
                color = (Color)outValue;
            }
            MenuItems = new ObservableCollection<MainPageMasterMenuItem>(new[]
            {
                new MainPageMasterMenuItem { Id = 0, Title = "Train", TargetType = typeof(PracticesPage), Icon = new FontImageSource() { FontFamily = "FontAwesomeSolid", Glyph="\uf140", Color=color} },
                new MainPageMasterMenuItem { Id = 1, Title = "History", TargetType = typeof(SessionsPage), Icon = new FontImageSource() { FontFamily = "FontAwesomeSolid", Glyph="\uf03a", Color=color} },
                new MainPageMasterMenuItem { Id = 2, Title = "Settings", TargetType = typeof(SettingsPage), Icon = new FontImageSource() { FontFamily = "FontAwesomeSolid", Glyph="\uf013", Color=color} }
            });
        }
    }
}
