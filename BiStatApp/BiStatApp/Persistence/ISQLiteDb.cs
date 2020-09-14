using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiStatApp.Persistence
{
	public interface ISQLiteDb
	{
		SQLiteAsyncConnection GetConnection();
	}
}
