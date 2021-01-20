using BiStatApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace BiStatApp.Persistence
{
	public class BiStatContext : DbContext
	{
		public DbSet<Session> Sessions { get; set; }
		public DbSet<ShootingBout> Bouts { get; set; }

		private readonly string _databasePath;

		public BiStatContext(string databasePath)
		{
			SQLitePCL.Batteries_V2.Init();
			_databasePath = databasePath;
			this.Database.Migrate();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Filename={_databasePath}");
		}
	}
}
