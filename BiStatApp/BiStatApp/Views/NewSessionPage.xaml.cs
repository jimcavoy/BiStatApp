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
    public partial class NewSessionPage : ContentPage
    {
        public NewSessionPage()
        {
            ViewModel = new NewSessionPageViewModel();
            InitializeComponent();
        }

        public NewSessionPageViewModel ViewModel
        {
            get { return BindingContext as NewSessionPageViewModel; }
            set { BindingContext = value; }
        }
    }
}