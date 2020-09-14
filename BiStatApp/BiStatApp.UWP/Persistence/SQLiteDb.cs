using BiStatApp.Persistence;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(BiStatApp.UWP.Persistence.SQLiteDb))]

namespace BiStatApp.UWP.Persistence
{
	public class SQLiteDb : ISQLiteDb
	{
		public SQLiteAsyncConnection GetConnection()
		{
			var documentsPath = ApplicationData.Current.LocalFolder.Path;
			var path = System.IO.Path.Combine(documentsPath, "BiStatApp.db3");
			return new SQLiteAsyncConnection(path);
		}
	}
}
