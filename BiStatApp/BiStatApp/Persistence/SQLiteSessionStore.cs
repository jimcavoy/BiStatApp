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

        public async Task<Session> AddSession(Session session)
        {
            Session ret;
            using (var context = new BiStatContext())
            {
                ret = context.Sessions.Add(session).Entity;

                foreach (var b in session.Bouts)
                {
                    b.SessionId = ret.Id;
                    context.Bouts.Add(b);
                }

                await context.SaveChangesAsync();
            }
            return ret;
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

        public async Task<IEnumerable<Practice>> GetPracticesAsync()
        {
            List<Practice> list = new List<Practice>();
            await Task.Run(() =>
            {
                using (var context = new BiStatContext())
                {
                    list = context.Practices.ToList();
                }
            });
            return list.AsEnumerable();
        }

        public void SeedData()
        {
            using (var context = new BiStatContext())
            {
                if (!context.Practices.Any())
                {
                    context.Practices.Add(new Practice { Name = "1 Shot Setup", Description = "Starting from behind the mat, setup in the prone or standing position and shoot on one target, then get up." });
                    context.Practices.Add(new Practice { Name = "5 Across", Description = "Shoot five across on a prone or standing target." });
                    context.Practices.Add(new Practice { Name = "Combo", Description = "A practices that combines skiing and shooting." });
                    context.Practices.Add(new Practice { Name = "Time Trail", Description = "A practice biathlon race." });
                    context.Practices.Add(new Practice { Name = "Open Training", Description = "Athlete defined practice." });
                    context.Practices.Add(new Practice { Name = "Race - Sprint", Description = "Sprint race format." });
                    context.Practices.Add(new Practice { Name = "Race - Pursuit", Description = "Pursuit race format." });
                    context.Practices.Add(new Practice { Name = "Race - Mass Start", Description = "Mass Start race format." });
                    context.Practices.Add(new Practice { Name = "Race - Individual", Description = "Individual race format." });
                    context.Practices.Add(new Practice { Name = "Race - Relay", Description = "Relay race format." });
                    context.Practices.Add(new Practice { Name = "Dry Fire", Description = "A dry fire practice." });

                    context.SaveChanges();
                }
            }
        }

    }
}
