using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public ImageSource BannerImage
        { 
            get
            {
                return ImageSource.FromResource("BiStatApp.Assets.Images.xamarin_logo.png");
            }
        }

    }
}
