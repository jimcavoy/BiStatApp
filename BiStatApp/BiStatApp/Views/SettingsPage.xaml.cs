using BiStatApp.ViewModels;
using BiStatApp.Persistence;

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

            var sessionStore = new SQLiteSessionStore();
            var pageService = new PageService() { MainPage = Navigation };

            BindingContext = new SettingsPageViewModel(sessionStore, pageService);
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

        private void OnExportButtonClicked(object sender, EventArgs e)
        {
            ViewModel.ExportDataCommand.Execute(e);
        }

        private void OnImportButtonClicked(object sender, EventArgs e)
        {
            ViewModel.ImportDataCommand.Execute(e);
        }
    }
}