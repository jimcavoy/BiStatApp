using BiStatApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BiStatApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new SessionsPage());
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
