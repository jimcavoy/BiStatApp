using BiStatApp.Models;
using BiStatApp.Persistence;
using BiStatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BiStatApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShootingBoutDetailPage : ContentPage
    {
        public ShootingBoutDetailPage(ShootingBoutViewModel viewModel)
        {
            InitializeComponent();

            var sessionStore = new SQLiteSessionStore();
            var pageService = new PageService();
            Title = (viewModel.Id == 0) ? "New Shooting Bout" : "Edit Shooting Bout";
            BindingContext = new ShootingBoutPageViewModel(viewModel ?? new ShootingBoutViewModel(), sessionStore, pageService);
        }

        public ShootingBoutPageViewModel ViewModel
        {
            get { return BindingContext as ShootingBoutPageViewModel; }
            set { BindingContext = value; }
        }
    }
}