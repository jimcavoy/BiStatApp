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

        public ShootingBoutDetailPage()
        {
            InitializeComponent();
            ViewModel = new ShootingBoutPageViewModel();
        }

        public ShootingBoutPageViewModel ViewModel
        {
            get { return BindingContext as ShootingBoutPageViewModel; }
            set { BindingContext = value; }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<ShootingBoutPageViewModel>
                (this, Events.ShootingBoutUpdated, OnShootingBoutUpdate);
        }

        protected void OnShootingBoutUpdate(ShootingBoutPageViewModel source)
        {
            Alpha.IsChecked = ViewModel.Bout.Alpha;
            Bravo.IsChecked = ViewModel.Bout.Bravo;
            Charlie.IsChecked = ViewModel.Bout.Charlie;
            Delta.IsChecked = ViewModel.Bout.Delta;
            Echo.IsChecked = ViewModel.Bout.Echo;
            bool isToggled = ViewModel.Bout.Position == ShootingBout.PositionEnum.STANDING;
            Position.IsToggled = isToggled;
            MessagingCenter.Unsubscribe<ShootingBoutPageViewModel>
               (this, Events.ShootingBoutUpdated);
        }

        private void BtnStopwatch_Clicked(object sender, EventArgs e)
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

        private void AdvanceSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            ViewModel.ShowAdvanceView = e.Value;
        }
    }
}