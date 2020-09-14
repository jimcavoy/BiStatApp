using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BiStatApp.Models
{
	public class ShootingBout
	{
		public enum PositionEnum
		{
			PRONE = 1,
			STANDING = 2
		}

		[Key]
		public int Id { get; set; }

		public PositionEnum Position { get; set; }

		public bool Alpha { get; set; }

		public bool Bravo { get; set; }

		public bool Charlie { get; set; }

		public bool Delta { get; set; }

		public bool Echo { get; set; }

		public int SessionId { get; set; }
		public Session Session { get; set; }
	}
}
