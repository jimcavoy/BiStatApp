﻿using BiStatApp.Persistence;
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
    public partial class SessionDetailPage : ContentPage
    {
        public SessionDetailPage(SessionViewModel viewModel)
        {
            InitializeComponent();

            var sessionStore = new SQLiteSessionStore();
            var pageService = new PageService();
            Title = (viewModel.Name == null) ? "New Session" : "Edit Session";
            BindingContext = new SessionDetailViewModel(viewModel ?? new SessionViewModel(), sessionStore, pageService);
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        void OnBoutSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectShootingBoutCommand.Execute(e.SelectedItem);
        }

        public SessionDetailViewModel ViewModel
        {
            get { return BindingContext as SessionDetailViewModel; }
            set { BindingContext = value; }
        }
    }
}