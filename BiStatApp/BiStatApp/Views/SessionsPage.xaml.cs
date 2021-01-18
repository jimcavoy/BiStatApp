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
    public partial class SessionsPage : ContentPage
    {
        public SessionsPage()
        {
            ViewModel = new SessionsPageViewModel();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
            ViewModel.LoadDataCommand.Execute(null);
        }

        public SessionsPageViewModel ViewModel
        {
            get { return BindingContext as SessionsPageViewModel; }
            set { BindingContext = value; }
        }
    }
}