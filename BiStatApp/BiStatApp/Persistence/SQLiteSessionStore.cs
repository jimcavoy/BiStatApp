using BiStatApp.Models;
using BiStatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BiStatApp.Persistence
{
	public class SQLiteSessionStore : ISessionStore
	{
		private SQLiteAsyncConnection _connection;
		public SQLiteSessionStore(ISQLiteDb db)
		{
			_connection = db.GetConnection();
			_connection.CreateTableAsync<Session>();
		}

		public async Task<IEnumerable<Session>> GetSessionsAsync()
		{
			return await _connection.Table<Session>().ToListAsync();
		}

		public async Task DeleteSession(Session session)
		{
			await _connection.DeleteAsync(session);
		}

		public async Task AddSession(Session session)
		{
			await _connection.InsertAsync(session);
		}

		public async Task UpdateSession(Session session)
		{
			await _connection.UpdateAsync(session);
		}

		public async Task<Session> GetSession(int id)
		{
			return await _connection.FindAsync<Session>(id);
		}

	}
}
