using BiStatApp.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BiStatApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendSessionPage : ContentPage
    {
        public SendSessionPage()
        {
            InitializeComponent();
            BindingContext = new SendSessionPageViewModel();
        }
    }
}