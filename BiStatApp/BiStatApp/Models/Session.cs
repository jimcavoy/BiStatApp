using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiStatApp.Models
{
	public class Session
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime DateTime { get; set; }

		public string Description { get; set; }

	}
}
