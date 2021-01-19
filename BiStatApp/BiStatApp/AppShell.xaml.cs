using BiStatApp.ViewModels;
using BiStatApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BiStatApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SessionDetailPage), typeof(SessionDetailPage));
            Routing.RegisterRoute(nameof(ReportPage), typeof(ReportPage));
        }

    }
}
