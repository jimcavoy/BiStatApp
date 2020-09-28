using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using Xamarin.Forms;

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
		[JsonIgnore]
		public int Id { get; set; }

		public PositionEnum Position { get; set; } = PositionEnum.PRONE;

		public bool Alpha { get; set; }

		public bool Bravo { get; set; }

		public bool Charlie { get; set; }

		public bool Delta { get; set; }

		public bool Echo { get; set; }

		[JsonIgnore]
		public int SessionId { get; set; }

		[JsonIgnore]
		public Session Session { get; set; }

	}
}
