using BiStatApp.Persistence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SQLite;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(BiStatApp.Droid.Persistence.SQLiteDb))]

namespace BiStatApp.Droid.Persistence
{
	public class SQLiteDb : ISQLiteDb
	{
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}