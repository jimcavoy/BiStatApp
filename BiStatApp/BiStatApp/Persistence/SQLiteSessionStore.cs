using BiStatApp.Models;
using BiStatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiStatApp.Persistence
{
	public class SQLiteSessionStore : ISessionStore
	{
		public SQLiteSessionStore()
		{

		}

		public async Task<IEnumerable<Session>> GetSessionsAsync()
		{
			List<Session> list = new List<Session>();
			await Task.Run(() => 
			{ 
				using (var context = new BiStatContext())
				{
					list = context.Sessions.ToList();
				}
			});

			return list.AsEnumerable();
		}

		public async Task DeleteSession(Session session)
		{
			using (var context = new BiStatContext())
			{
				var origSession = context.Sessions
					.Where(s => s.Id == session.Id).FirstOrDefault();
				context.Remove(origSession);
				await context.SaveChangesAsync();
			}
		}

		public async Task AddSession(Session session)
		{
			using (var context = new BiStatContext())
			{
				context.Sessions.Add(session);

				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateSession(Session session)
		{
			using (var context = new BiStatContext())
			{
				var origSession = context.Sessions
					.Where(s => s.Id == session.Id).FirstOrDefault();

				origSession.Name = session.Name;
				origSession.Description = session.Description;
				origSession.DateTime = session.DateTime;

				await context.SaveChangesAsync();
			}
		}

		public async Task<Session> GetSession(int id)
		{
			Session session = new Session();
			await Task.Run(() =>
			{
				using (var context = new BiStatContext())
				{
					session = context.Sessions
						.Where(s => s.Id == id)
						.FirstOrDefault();
				}
			});
			return session;
		}

	}
}
