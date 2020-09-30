using BiStatApp.Views;
using BiStatApp.Persistence;
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

			MainPage = new BiStatApp.Views.MainPage();

			var database = new SQLiteSessionStore();
			database.SeedData();
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
