using BiStatApp.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BiStatApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            var pageService = new PageService() { MainPage = Navigation };

            BindingContext = new SettingsPageViewModel(pageService);
        }

        public SettingsPageViewModel ViewModel
        {
            get { return BindingContext as SettingsPageViewModel; }
            set { BindingContext = value; }
        }

        private void OnSendButtonClicked(object sender, EventArgs e)
        {
            ViewModel.SendReportCommand.Execute(e);
        }
    }
}