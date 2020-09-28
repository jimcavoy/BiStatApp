using BiStatApp.Models;
using BiStatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
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
                    list = context.Sessions.Include(s => s.Bouts).ToList();
                }
            });

            return list.OrderBy(d => d.DateTime).Reverse().AsEnumerable();
        }

        public async Task DeleteSession(Session session)
        {
            using (var context = new BiStatContext())
            {
                var origSession = context.Sessions
                    .Where(s => s.Id == session.Id)
                    .FirstOrDefault();
                context.Remove(origSession);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddSession(Session session)
        {
            using (var context = new BiStatContext())
            {
                context.Sessions.Add(session);

                foreach (var b in session.Bouts)
                {
                    context.Bouts.Add(b);
                }

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
                        .Include(s => s.Bouts)
                        .FirstOrDefault();
                }
            });
            return session;
        }

        public async Task<ShootingBout> GetShootingBout(int id)
        {
            ShootingBout bout = new ShootingBout();
            await Task.Run(() =>
            {
                using (var context = new BiStatContext())
                {
                    bout = context.Bouts
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
                }
            });
            return bout;
        }

        public async Task AddShootingBout(ShootingBout bout)
        {
            using (var context = new BiStatContext())
            {
                context.Bouts.Add(bout);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateShootingBout(ShootingBout bout)
        {
            using (var context = new BiStatContext())
            {
                var origBout = context.Bouts
                    .Where(b => b.Id == bout.Id).FirstOrDefault();

                if (origBout != null)
                {
                    origBout.Position = bout.Position;
                    origBout.Alpha = bout.Alpha;
                    origBout.Bravo = bout.Bravo;
                    origBout.Charlie = bout.Charlie;
                    origBout.Delta = bout.Delta;
                    origBout.Echo = bout.Echo;

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteShootingBout(ShootingBout bout)
        {
            if (bout.Id == 0)
                return;

            using (var context = new BiStatContext())
            {
                context.Bouts.Remove(bout);
                await context.SaveChangesAsync();
            }
        }

    }
}
