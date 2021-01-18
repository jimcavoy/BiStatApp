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

			DependencyService.Register<SQLiteSessionStore>();
			MainPage = new AppShell();
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
