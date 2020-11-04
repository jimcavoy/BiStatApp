using BiStatApp.Models;
using BiStatApp.Persistence;
using BiStatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BiStatApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShootingBoutDetailPage : ContentPage
    {
        Timer _timer;
        decimal _elapsedTime;

        public ShootingBoutDetailPage(ShootingBoutViewModel viewModel)
        {
            InitializeComponent();

            var sessionStore = new SQLiteSessionStore();
            var pageService = new PageService() { MainPage = Navigation };
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
            Position.IsToggled = ViewModel.Bout.Position != ShootingBout.PositionEnum.PRONE;
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

        private void btnStopwatch_Clicked(object sender, EventArgs e)
        {
            if (btnStopwatch.BackgroundColor == Color.Green)
            {
                btnStopwatch.Text = "Stop";
                btnStopwatch.BackgroundColor = Color.Red;
                ViewModel.Bout.Duration = 0;
                _elapsedTime = 0;
                _timer = new Timer(TimerProc, null, 0, 100);
            }
            else
            {
                _timer.Dispose();
                btnStopwatch.Text = "Start";
                btnStopwatch.BackgroundColor = Color.Green;
            }
        }

        private void TimerProc(object state)
        {
            _elapsedTime += Convert.ToDecimal(0.1);
            ViewModel.Bout.Duration = _elapsedTime;
        }
    }
}