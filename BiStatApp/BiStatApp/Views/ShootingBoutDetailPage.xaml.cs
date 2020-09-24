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
        
        protected override void OnAppearing()
        {
            Alpha.IsChecked = ViewModel.Bout.Alpha;
            Bravo.IsChecked = ViewModel.Bout.Bravo;
            Charlie.IsChecked = ViewModel.Bout.Charlie;
            Delta.IsChecked = ViewModel.Bout.Delta;
            Echo.IsChecked = ViewModel.Bout.Echo;
            Position.IsToggled = ViewModel.Bout.Position == ShootingBout.PositionEnum.PRONE ? false : true;
            base.OnAppearing();
        }

        private void Alpha_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ViewModel.AlphaCheckedCommand.Execute(e.Value);
        }

        private void Bravo_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ViewModel.BravoCheckedCommand.Execute(e.Value);
        }

        private void Charlie_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ViewModel.CharlieCheckedCommand.Execute(e.Value);
        }

        private void Delta_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ViewModel.DeltaCheckedCommand.Execute(e.Value);
        }

        private void Echo_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ViewModel.EchoCheckedCommand.Execute(e.Value);
        }

        private void Position_Toggled(object sender, ToggledEventArgs e)
        {
            ViewModel.PositionChangedCommand.Execute(e.Value);
        }
    }
}