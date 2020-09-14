using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BiStatApp.Models;

namespace BiStatApp.ViewModels
{
	public interface ISessionStore
	{
		Task<IEnumerable<Session>> GetSessionsAsync();

		Task<Session> GetSession(int id);

		Task AddSession(Session session);

		Task UpdateSession(Session session);

		Task DeleteSession(Session session);
	}
}
