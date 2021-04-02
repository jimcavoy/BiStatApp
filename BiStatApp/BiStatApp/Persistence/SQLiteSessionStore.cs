using BiStatApp.Models;
using BiStatApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace BiStatApp.Persistence
{
    public class SQLiteSessionStore : ISessionStore
    {
        private readonly List<Practice> _practices = new List<Practice>();

        private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "BiStatApp.db3");

        public SQLiteSessionStore()
        {

        }

        public async Task<IEnumerable<Session>> GetSessionsAsync()
        {
            List<Session> list = new List<Session>();
            await Task.Run(() =>
            {
                using var context = new BiStatContext(_dbPath);
                list = context.Sessions.Include(s => s.Bouts).ToList();
            });

            return list.OrderBy(d => d.DateTime).Reverse().AsEnumerable();
        }

        public async Task DeleteSession(Session session)
        {
            using var context = new BiStatContext(_dbPath);
            var origSession = context.Sessions
                .Where(s => s.Id == session.Id)
                .FirstOrDefault();
            context.Remove(origSession);
            await context.SaveChangesAsync();
        }

        public async Task<Session> AddSession(Session session)
        {
            Session ret;
            using (var context = new BiStatContext(_dbPath))
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
            using (var context = new BiStatContext(_dbPath))
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
                using (var context = new BiStatContext(_dbPath))
                {
                    session = context.Sessions
                        .Where(s => s.Id == id)
                        .Include(s => s.Bouts)
                        .FirstOrDefault();
                }
            });
            return session;
        }

        public async Task<BiStatDocument> GetDocument()
        {
            BiStatDocument doc = new BiStatDocument();
            await Task.Run(() =>
            {
                using (var context = new BiStatContext(_dbPath))
                {
                    var list = context.Sessions.Include(s => s.Bouts).ToList();
                    foreach (var s in list)
                    {
                        doc.Sessions.Add(s);
                    }
                }
            });
            return doc;
        }

        public async Task SetDocument(BiStatDocument doc)
        {
            foreach(var session in doc.Sessions)
            {
                await AddSession(session);
            }
        }

        public async Task<ShootingBout> GetShootingBout(int id)
        {
            ShootingBout bout = new ShootingBout();
            await Task.Run(() =>
            {
                using (var context = new BiStatContext(_dbPath))
                {
                    bout = context.Bouts
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
                }
            });
            return bout;
        }

        public async Task<ShootingBout> AddShootingBout(ShootingBout bout)
        {
            ShootingBout ret = null;
            using (var context = new BiStatContext(_dbPath))
            {
                ret = context.Bouts.Add(bout).Entity;
                await context.SaveChangesAsync();
            }
            return ret;
        }

        public async Task UpdateShootingBout(ShootingBout bout)
        {
            using (var context = new BiStatContext(_dbPath))
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
                    origBout.StartHeartRate = bout.StartHeartRate;
                    origBout.EndHeartRate = bout.EndHeartRate;
                    origBout.Duration = bout.Duration;

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteShootingBout(ShootingBout bout)
        {
            if (bout.Id == 0)
                return;

            using (var context = new BiStatContext(_dbPath))
            {
                context.Bouts.Remove(bout);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Practice>> GetPracticesAsync()
        {
            SeedData();
            return await Task.Run(() => _practices);
        }

        public void SeedData()
        {
            if (_practices.Count == 0)
            {
                _practices.Add(new Practice { Name = "1 Shot Setup", Description = "Starting from behind the mat, setup in the prone or standing position and shoot on one target, then get up." });
                _practices.Add(new Practice { Name = "5 Across", Description = "Shoot five across on a prone or standing target." });
                _practices.Add(new Practice { Name = "Combo", Description = "A practices that combines skiing and shooting." });
                _practices.Add(new Practice { Name = "Time Trial", Description = "A practice biathlon race." });
                _practices.Add(new Practice { Name = "Open Training", Description = "Athlete defined practice." });
                _practices.Add(new Practice { Name = "Race - Sprint", Description = "Sprint race format." });
                _practices.Add(new Practice { Name = "Race - Pursuit", Description = "Pursuit race format." });
                _practices.Add(new Practice { Name = "Race - Mass Start", Description = "Mass Start race format." });
                _practices.Add(new Practice { Name = "Race - Individual", Description = "Individual race format." });
                _practices.Add(new Practice { Name = "Race - Relay", Description = "Relay race format." });
                _practices.Add(new Practice { Name = "Dry Fire", Description = "A dry fire practice." });
            }
        }

    }
}
