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
    public partial class ReportPage : ContentPage
    {
        public ReportPage(IEnumerable<SessionViewModel> sessions)
        {
            InitializeComponent();

            BindingContext = new ReportPageViewModel(sessions);
        }

        public ReportPageViewModel ViewModel
        {
            get { return BindingContext as ReportPageViewModel; }
            set { BindingContext = value; }
        }
    }
}