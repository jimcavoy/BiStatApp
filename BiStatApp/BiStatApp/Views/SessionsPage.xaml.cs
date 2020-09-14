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
	public partial class SessionsPage : ContentPage
	{
		public SessionsPage()
		{
			var sessionStore = new SQLiteSessionStore();
			var pageService = new PageService();

			ViewModel = new SessionsPageViewModel(sessionStore, pageService);

			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			ViewModel.LoadDataCommand.Execute(null);
			base.OnAppearing();
		}

		void OnSessionSelected(object sender, SelectedItemChangedEventArgs e)
		{
			ViewModel.SelectSessionCommand.Execute(e.SelectedItem);
		}

		public SessionsPageViewModel ViewModel
		{
			get { return BindingContext as SessionsPageViewModel; }
			set { BindingContext = value; }
		}
	}
}