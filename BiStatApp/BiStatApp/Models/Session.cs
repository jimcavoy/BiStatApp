using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BiStatApp.Models
{
	public class Session
	{
		[Key]
		[System.Text.Json.Serialization.JsonIgnore]
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime DateTime { get; set; }

		public string Description { get; set; }

		public List<ShootingBout> Bouts { get; set; } = new List<ShootingBout>();
	}
}
