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

		public BiStatContext()
		{
			SQLitePCL.Batteries_V2.Init();

			this.Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string dbPath = Path.Combine(FileSystem.AppDataDirectory, "BiStatApp.db3");

			optionsBuilder.UseSqlite($"Filename={dbPath}");
		}
	}
}
