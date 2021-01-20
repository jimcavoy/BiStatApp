using BiStatApp.ViewModels;

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

        public SessionDetailViewModel ViewModel
        {
            get { return BindingContext as SessionDetailViewModel; }
            set { BindingContext = value; }
        }
    }
}