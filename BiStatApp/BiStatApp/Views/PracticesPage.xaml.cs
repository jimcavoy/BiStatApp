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
            ViewModel = new PracticesPageViewModel();
            Title = "Add New Session";
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        public PracticesPageViewModel ViewModel
        {
            get { return BindingContext as PracticesPageViewModel; }
            set { BindingContext = value; }
        }
    }
}