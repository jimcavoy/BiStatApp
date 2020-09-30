using BiStatApp.Persistence;
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
    public partial class PracticesPage : ContentPage
    {
        public PracticesPage()
        {
            var sessionStore = new SQLiteSessionStore();
            var pageService = new PageService()
            {
                MainPage = Navigation
            };

            ViewModel = new PracticesPageViewModel(sessionStore, pageService);

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectPracticeCommand.Execute(e.SelectedItem);
        }

        public PracticesPageViewModel ViewModel
        {
            get { return BindingContext as PracticesPageViewModel; }
            set { BindingContext = value; }
        }
    }
}