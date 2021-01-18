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
    public partial class SessionDetailPage : ContentPage
    {

        public SessionDetailPage()
        {
            InitializeComponent();
            ViewModel = new SessionDetailViewModel();
        }

        public SessionDetailPage(SessionViewModel viewModel)
        {
            InitializeComponent();
            Title = (viewModel.Name == null) ? "New Session" : "Edit Session";
            ViewModel = new SessionDetailViewModel(viewModel);
        }

        public SessionDetailViewModel ViewModel
        {
            get { return BindingContext as SessionDetailViewModel; }
            set { BindingContext = value; }
        }
    }
}